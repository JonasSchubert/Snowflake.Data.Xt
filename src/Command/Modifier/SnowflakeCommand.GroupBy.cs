namespace Snowflake.Data.Xt;

public partial class SnowflakeCommand<T>
  where T : class
{
  /// <summary>
  /// Adds a group by clause.
  /// https://docs.snowflake.com/en/sql-reference/constructs/group-by
  /// </summary>
  /// <param name="predicate">The group by predicate.</param>
  /// <returns>The snowflake command.</returns>
  /// <exception cref="InvalidOperationException">Command already has a group by clause!</exception>
  public SnowflakeCommand<T> GroupBy<TGroupBy>(Expression<Func<T, TGroupBy>> predicate)
  {
    if (this.Sql.Contains("GROUP BY"))
    {
      throw new InvalidOperationException("Command already has a group by clause!");
    }

    var groupByBody = string.Join(", ", GroupByRegex()
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

      groupByBody = groupByBody.Replace($"{property.Key.Name}", $"{propertyTableAlias}.{propertyName}");
    }

    this.SqlBuilder.Append($" GROUP BY {groupByBody.Trim()}");

    return this;
  }

  /// <summary>
  /// Adds a group by clause.
  /// https://docs.snowflake.com/en/sql-reference/constructs/group-by
  /// </summary>
  /// <param name="groupBy">The group by clause.</param>
  /// <returns>The snowflake command.</returns>
  /// <exception cref="InvalidOperationException">Command already has a group by clause!</exception>
  /// <exception cref="ArgumentNullException">Value for group by clause may not be empty!</exception>
  /// <exception cref="ArgumentException">Group By may only contain valid properties.</exception>
  public SnowflakeCommand<T> GroupBy(string groupBy)
  {
    if (this.Sql.Contains("GROUP BY"))
    {
      throw new InvalidOperationException("Command already has a group by clause!");
    }

    if (string.IsNullOrWhiteSpace(groupBy))
    {
      throw new ArgumentNullException(nameof(groupBy), "Value for group by clause may not be empty!");
    }

    var regexTestValue = groupBy.Replace("GROUP BY", string.Empty).Trim();
    if (!this.ValidPropertiesRegex.IsMatch(regexTestValue))
    {
      throw new ArgumentException($"Group By may only contain valid properties ({string.Join(", ", this.ValidProperties)})! \"{regexTestValue}\" is not allowed!");
    }

    this.SqlBuilder.Append($" {(groupBy.Trim().StartsWith("GROUP BY", ignoreCase: true, null) ? groupBy.Trim() : $"GROUP BY {groupBy.Trim()}")}");

    return this;
  }

  [GeneratedRegex("new <>f__AnonymousType[0-9]{1,}`[0-9]{1,}")]
  private static partial Regex GroupByRegex();
}
