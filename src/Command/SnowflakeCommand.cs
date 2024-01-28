//-----------------------------------------------------------------------
// <copyright file="SnowflakeCommand.cs" company="Jonas Schubert">
//     Copyright (c) Jonas Schubert. All rights reserved.
// </copyright>
// <author>Jonas Schubert</author>
//-----------------------------------------------------------------------

using System.Reflection;
using System.Text;
using log4net;

namespace Snowflake.Data.Xt;

/// <summary>
/// The snowflake command.
/// </summary>
/// <typeparam name="T">The generic type. This is used to parse properties for the query.</typeparam>
public partial class SnowflakeCommand<T>
  where T : class
{
  /// <summary>
  /// An array of chars used for table aliases in the final command.
  /// </summary>
#pragma warning disable CA2211, MA0069, SA1306, SA1401
  protected static char[] Alphabet = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

  /// <summary>
  /// The logger.
  /// </summary>
  protected static ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);
#pragma warning restore CA2211, MA0069, SA1306, SA1401

  /// <summary>
  /// The snowflake database connection.
  /// </summary>
  private readonly SnowflakeDbConnection? snowflakeDbConnection;

  /// <summary>
  /// Initializes a new instance of the <see cref="SnowflakeCommand{T}"/> class.
  /// </summary>
  /// <exception cref="ArgumentNullException">If either env variable for database or schema is not set, an argument null exception is being thrown.</exception>
  public SnowflakeCommand()
    : this(
      database: Environment.GetEnvironmentVariable("SNOWFLAKE_DATABASE") !,
      schema: Environment.GetEnvironmentVariable("SNOWFLAKE_SCHEMA") !,
      snowflakeDbConnection: null)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="SnowflakeCommand{T}"/> class.
  /// </summary>
  /// <param name="database">The database.</param>
  /// <param name="schema">The schema.</param>
  public SnowflakeCommand(string database, string schema)
    : this(database, schema, snowflakeDbConnection: null)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="SnowflakeCommand{T}"/> class.
  /// </summary>
  /// <param name="snowflakeDbConnection">The snowflake database connection.</param>
  public SnowflakeCommand(SnowflakeDbConnection snowflakeDbConnection)
    : this(
      database: Environment.GetEnvironmentVariable("SNOWFLAKE_DATABASE") !,
      schema: Environment.GetEnvironmentVariable("SNOWFLAKE_SCHEMA") !,
      snowflakeDbConnection)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="SnowflakeCommand{T}"/> class.
  /// </summary>
  /// <param name="database">The database.</param>
  /// <param name="schema">The schema.</param>
  /// <param name="snowflakeDbConnection">The snowflake database connection.</param>
  public SnowflakeCommand(string database, string schema, SnowflakeDbConnection? snowflakeDbConnection)
  {
    this.snowflakeDbConnection = snowflakeDbConnection;
#pragma warning disable SA1503
    if (string.IsNullOrWhiteSpace(database)) throw new ArgumentException(string.Format("'{0}' is not a valid value for database.", database), nameof(database));
    if (string.IsNullOrWhiteSpace(schema)) throw new ArgumentException(string.Format("'{0}' is not a valid value for schema.", schema), nameof(schema));
#pragma warning restore SA1503

    var alphabetIndex = 0;
    this.Table = (SnowflakeTableAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(SnowflakeTableAttribute)) !;
    if (string.IsNullOrWhiteSpace(this.Table.Name))
    {
      this.Table.Name = typeof(T).Name;
    }

    if (string.IsNullOrWhiteSpace(this.Table.Alias))
    {
      this.Table.Alias = SnowflakeCommand<T>.Alphabet[alphabetIndex].ToString();
      alphabetIndex++;
    }

    this.Joins = Attribute.GetCustomAttributes(typeof(T), typeof(SnowflakeJoinAttribute)).Cast<SnowflakeJoinAttribute>()
      .Select(attribute =>
      {
        if (string.IsNullOrWhiteSpace(attribute.Alias))
        {
          attribute.Alias = SnowflakeCommand<T>.Alphabet[alphabetIndex].ToString();
          alphabetIndex++;
        }

        return attribute;
      })
      .ToList();

    this.Properties = typeof(T)
      .GetProperties()
      .Where(propertyInfo => propertyInfo.GetCustomAttributes(typeof(SnowflakeColumnAttribute), inherit: true).Length > 0)
      .Select(propertyInfo =>
      {
        var attribute = (SnowflakeColumnAttribute)propertyInfo.GetCustomAttributes(typeof(SnowflakeColumnAttribute), inherit: true).Single();
        attribute.Name = string.IsNullOrWhiteSpace(attribute.Name) ? propertyInfo.Name : attribute.Name;

        return (propertyInfo, attribute);
      })
      .ToDictionary(_ => _.propertyInfo, _ => _.attribute);
    this.ValidProperties = this.Properties
      .Select(property =>
      {
        var propertyTableAlias = string.IsNullOrWhiteSpace(property.Value.Table)
          ? this.Table.Alias
          : this.Joins.Single(join => string.Equals(join.Table, property.Value.Table, StringComparison.Ordinal)).Alias;

        return $"{propertyTableAlias}.{property.Value.Name}";
      })
      .ToList();
    this.ValidPropertiesRegex = new Regex($"^({string.Join('|', this.ValidProperties)})$", RegexOptions.None, TimeSpan.FromSeconds(3));

    this.SqlBuilder = new StringBuilder("SELECT ");
    this.AddColumns();
    this.AddFrom(database, schema);
    this.AddJoins(database, schema);
  }

  /// <summary>
  /// Gets the list of joins.
  /// </summary>
  public IList<SnowflakeJoinAttribute> Joins { get; }

  /// <summary>
  /// Gets or sets the parameter list for the query.
  /// </summary>
  public IList<(string, DbType, object)> ParameterList { get; set; } = new List<(string, DbType, object)>();

  /// <summary>
  /// Gets the properties dictionary.
  /// </summary>
  public IDictionary<PropertyInfo, SnowflakeColumnAttribute> Properties { get; }

  /// <summary>
  /// Gets the SQL command.
  /// </summary>
  public string Sql
  {
    get { return this.SqlBuilder.ToString().Trim(); }
  }

  /// <summary>
  /// Gets the table.
  /// </summary>
  public SnowflakeTableAttribute Table { get; }

  /// <summary>
  /// Gets the SQL builder.
  /// </summary>
  protected StringBuilder SqlBuilder { get; }

  /// <summary>
  /// Gets the columns.
  /// </summary>
  protected string Columns
  {
    get
    {
      var columns = new StringBuilder();

      foreach (KeyValuePair<PropertyInfo, SnowflakeColumnAttribute> property in this.Properties)
      {
        var column = property.Value;
        var table = column.Table ?? this.Table.Name;
        var tableAlias = string.Equals(this.Table.Name, table, StringComparison.Ordinal) ? this.Table.Alias ?? string.Empty
          : this.Joins.SingleOrDefault(join => string.Equals(join.Table, table, StringComparison.Ordinal))?.Alias ?? string.Empty;

        columns.Append($"{tableAlias}{(string.IsNullOrWhiteSpace(tableAlias) ? string.Empty : ".")}{column.Name}, ");
      }

      columns.Remove(columns.Length - 2, 2); // Remove last ", " to avoid query issues

      return columns.ToString();
    }
  }

  /// <summary>
  /// Gets the valid properties.
  /// </summary>
  protected IList<string> ValidProperties { get; }

  /// <summary>
  /// Gets the regular expression for valid properties.
  /// </summary>
  protected Regex ValidPropertiesRegex { get; }

  /// <summary>
  /// Adds the columns to the SQL command.
  /// </summary>
  protected void AddColumns()
  {
    this.SqlBuilder.Append(this.Columns);
  }

  /// <summary>
  /// Adds the FROM clause to the SQL command.
  /// </summary>
  /// <param name="database">The database.</param>
  /// <param name="schema">The schema.</param>
  protected void AddFrom(string database, string schema)
  {
    this.SqlBuilder.Append($" FROM {database}.{schema}.{this.Table.Name}{(string.IsNullOrWhiteSpace(this.Table.Alias) ? string.Empty : $" AS {this.Table.Alias}")}");
  }

  /// <summary>
  /// Adds joins to the SQL command.
  /// </summary>
  /// <param name="database">The database.</param>
  /// <param name="schema">The schema.</param>
  protected void AddJoins(string database, string schema)
  {
    foreach (SnowflakeJoinAttribute join in this.Joins)
    {
      this.SqlBuilder.Append($" {join.Type} JOIN {database}.{schema}.{join.Table} AS {join.Alias} ON {join.Condition}");
    }
  }

  /// <summary>
  /// Writes an information log if enabled.
  /// </summary>
  /// <param name="text">The text to log.</param>
  protected void WriteLogInformation(string text)
  {
    if (string.Equals(Environment.GetEnvironmentVariable("SNOWFLAKE_LOG_ENABLED") ?? "true", "true", StringComparison.Ordinal))
    {
      Log.Info(text);
    }
  }
}
