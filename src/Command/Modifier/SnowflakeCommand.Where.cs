namespace Snowflake.Data.Xt;

public partial class SnowflakeCommand<T>
  where T : class
{
  /// <summary>
  /// Adds a where clause.
  /// https://docs.snowflake.com/en/sql-reference/constructs/where
  /// </summary>
  /// <param name="predicate">The where predicate.</param>
  /// <returns>The snowflake command.</returns>
  /// <exception cref="InvalidOperationException">Command already has a where clause!</exception>
  public SnowflakeCommand<T> Where(Expression<Func<T, bool>> predicate)
  {
    if (this.Sql.Contains("WHERE"))
    {
      throw new InvalidOperationException("Command already has a where clause!");
    }

    var whereBody = predicate.Body
      .ToString()
      .Replace($"{predicate.Parameters[0].Name}.", string.Empty);
    foreach (var item in new (string, string)[]
      {
          ("AndAlso", "AND"),
          ("OrElse", "OR"),
          ("==", "="),
        // TODO Add additional mappings, e.g. like https://github.com/phnx47/dapper-repositories/tree/main/src/SqlGenerator
      })
    {
      whereBody = whereBody.Replace(item.Item1, item.Item2);
    }

    foreach (var property in this.Properties)
    {
      var propertyName = property.Value.Name;
      var propertyTableAlias = string.IsNullOrWhiteSpace(property.Value.Table)
        ? this.Table.Alias
        : this.Joins.Single(join => join.Table == property.Value.Table).Alias;

      whereBody = whereBody.Replace($"{property.Key.Name}", $"{propertyTableAlias}.{propertyName}");
    }

    this.SqlBuilder.Append($" WHERE {whereBody.Trim()}");

    return this;
  }

  /// <summary>
  /// Adds a where clause.
  /// https://docs.snowflake.com/en/sql-reference/constructs/where
  /// </summary>
  /// <param name="where">The where filter.</param>
  /// <returns>The snowflake command.</returns>
  /// <exception cref="InvalidOperationException">Command already has a where clause!</exception>
  /// <exception cref="ArgumentNullException">Value for where clause may not be empty!</exception>
  public SnowflakeCommand<T> Where(string where)
  {
    if (this.Sql.Contains("WHERE"))
    {
      throw new InvalidOperationException("Command already has a where clause!");
    }

    if (string.IsNullOrWhiteSpace(where))
    {
      throw new ArgumentNullException(nameof(where), "Value for where clause may not be empty!");
    }

    this.SqlBuilder.Append($" {(where.Trim().StartsWith("WHERE", ignoreCase: true, null) ? where.Trim() : $"WHERE {where.Trim()}")}");

    return this;
  }
}
