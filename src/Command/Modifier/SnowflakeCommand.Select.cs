namespace Snowflake.Data.Xt;

public partial class SnowflakeCommand<T>
  where T : class
{
  /// <summary>
  /// Adds a select filter to only query for limited columns.
  /// https://docs.snowflake.com/en/sql-reference/sql/select
  /// </summary>
  /// <param name="predicate">The select predicate.</param>
  /// <returns>The snowflake command.</returns>
  public SnowflakeCommand<T> Select<TGroupBy>(Expression<Func<T, TGroupBy>> predicate)
  {
    var selectBody = string.Join(", ", new Regex("new <>f__AnonymousType[0-9]{1,}`[0-9]{1,}")
      .Replace(predicate.Body
        .ToString()
        .Replace($"{predicate.Parameters[0].Name}.", string.Empty), string.Empty)
      .Replace("(", string.Empty)
      .Replace(")", string.Empty)
      .Split(',')
      .Select(_ => _.Split("=")[0].Trim()));

    foreach (var property in this.Properties)
    {
      var propertyName = property.Value.Name;
      var propertyTableAlias = string.IsNullOrWhiteSpace(property.Value.Table)
        ? this.Table.Alias
        : this.Joins.Single(join => join.Table == property.Value.Table).Alias;

      selectBody = selectBody.Replace($"{property.Key.Name}", $"{propertyTableAlias}.{propertyName}");
    }

    this.SqlBuilder.Replace(this.Columns, selectBody.Trim());

    return this;
  }
}
