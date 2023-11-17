using System.Reflection;
using System.Text;
using System.Text.Json;

using log4net;

namespace Snowflake.Data.Xt;

public partial class SnowflakeCommand<T>
  where T : class
{
  protected readonly static char[] Alphabet = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

  protected readonly static ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

  public SnowflakeCommand()
    : this(
      Environment.GetEnvironmentVariable("SNOWFLAKE_DATABASE") ?? throw new ArgumentNullException("database"),
      Environment.GetEnvironmentVariable("SNOWFLAKE_SCHEMA") ?? throw new ArgumentNullException("schema"))
  {
  }

  public SnowflakeCommand(string database, string schema)
  {
    this.SqlBuilder = new StringBuilder();
    this.SqlBuilder.Append("SELECT ");

    this.Properties = typeof(T)
      .GetProperties()
      .Where(propertyInfo => propertyInfo.GetCustomAttributes(typeof(SnowflakeColumnAttribute), true).Length > 0)
      .ToDictionary(
        propertyInfo => propertyInfo,
        propertyInfo => (SnowflakeColumnAttribute)propertyInfo.GetCustomAttributes(typeof(SnowflakeColumnAttribute), true).Single())
      .Select(item =>
      {
        if (string.IsNullOrWhiteSpace(item.Value.Name))
        {
          item.Value.Name = JsonNamingPolicy.SnakeCaseUpper.ConvertName(item.Key.Name);
        }

        return item;
      })
      .ToDictionary();

    var alphabetIndex = 0;

    this.Table = (SnowflakeTableAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(SnowflakeTableAttribute))!;
    if (string.IsNullOrWhiteSpace(this.Table.Name))
    {
      this.Table.Name = JsonNamingPolicy.SnakeCaseUpper.ConvertName(typeof(T).Name);
    }

    if (string.IsNullOrWhiteSpace(this.Table.Alias))
    {
      this.Table.Alias = SnowflakeCommand<T>.Alphabet[alphabetIndex].ToString();
      alphabetIndex++;
    }

    this.Joins = Attribute.GetCustomAttributes(typeof(T), typeof(SnowflakeJoinAttribute))
      .Select(_ => (SnowflakeJoinAttribute)_)
      .Select(_ =>
      {
        if (string.IsNullOrWhiteSpace(_.Alias))
        {
          _.Alias = SnowflakeCommand<T>.Alphabet[alphabetIndex].ToString();
          alphabetIndex++;
        }

        return _;
      })
      .ToList();

    this.ValidProperties = this.Properties
      .Select(property =>
      {
        var propertyName = property.Value.Name;
        var propertyTableAlias = string.IsNullOrWhiteSpace(property.Value.Table)
          ? this.Table.Alias
          : this.Joins.Single(join => join.Table == property.Value.Table).Alias;

        return $"{propertyTableAlias}.{propertyName}";
      })
      .ToList();
    this.ValidPropertiesRegex = new Regex($"^({string.Join("|", this.ValidProperties)})$");

    this.AddColumns();
    this.AddFrom(database, schema);
    this.AddJoins(database, schema);
  }

  public IList<SnowflakeJoinAttribute> Joins { get; }

  public Dictionary<PropertyInfo, SnowflakeColumnAttribute> Properties { get; }

  public string Sql { get { return this.SqlBuilder.ToString().Trim(); } }

  public SnowflakeTableAttribute Table { get; }

  protected StringBuilder SqlBuilder { get; }

  protected string Columns
  {
    get
    {
      var columns = new StringBuilder();

      foreach (KeyValuePair<PropertyInfo, SnowflakeColumnAttribute> property in this.Properties)
      {
        var column = property.Value;
        var table = column.Table ?? this.Table.Name;
        var tableAlias = this.Table.Name == table
          ? this.Table.Alias ?? string.Empty
          : this.Joins.SingleOrDefault(join => join.Table == table)?.Alias ?? string.Empty;

        columns.Append($"{tableAlias}{(string.IsNullOrWhiteSpace(tableAlias) ? string.Empty : ".")}{column.Name ?? throw new ArgumentException(nameof(column.Name), "Name of column is not set!")}, ");
      }

      columns.Remove(columns.Length - 2, 2); // Remove last ", " to avoid query issues

      return columns.ToString();
    }
  }

  protected IList<string> ValidProperties { get; }

  protected Regex ValidPropertiesRegex { get; }

  protected void AddColumns()
  {
    this.SqlBuilder.Append(this.Columns);
  }

  protected void AddFrom(string database, string schema)
  {
    this.SqlBuilder.Append($" FROM {database}.{schema}.{this.Table.Name}{(string.IsNullOrWhiteSpace(this.Table.Alias) ? string.Empty : $" AS {this.Table.Alias}")}");
  }

  protected void AddJoins(string database, string schema)
  {
    foreach (SnowflakeJoinAttribute join in this.Joins)
    {
      this.SqlBuilder.Append($" {join.Type} JOIN {database}.{schema}.{join.Table} AS {join.Alias} ON {join.Condition}");
    }
  }

  protected void WriteLogInformation(string text)
  {
    if ((Environment.GetEnvironmentVariable("SNOWFLAKE_LOG_ENABLED") ?? "true") == "true")
    {
      Log.Info(text);
    }
  }
}
