//-----------------------------------------------------------------------
// <copyright file="SnowflakeCommand.GroupBy.cs" company="Jonas Schubert">
//     Copyright (c) Jonas Schubert. All rights reserved.
// </copyright>
// <author>EWP Team Fürth</author>
//-----------------------------------------------------------------------

using System.Globalization;

namespace Snowflake.Data.Xt;

/// <summary>
/// The snowflake command.
/// </summary>
/// <typeparam name="T">The generic type. This is used to parse properties for the query.</typeparam>
public partial class SnowflakeCommand<T>
  where T : class
{
  /// <summary>
  /// Adds a group by clause.
  /// https://docs.snowflake.com/en/sql-reference/constructs/group-by .
  /// </summary>
  /// <typeparam name="TGroupBy">The generic type.</typeparam>
  /// <param name="predicate">The group by predicate.</param>
  /// <returns>The snowflake command.</returns>
  /// <exception cref="InvalidOperationException">Command already has a group by clause.</exception>
  public SnowflakeCommand<T> GroupBy<TGroupBy>(Expression<Func<T, TGroupBy>> predicate)
  {
    if (this.Sql.Contains("GROUP BY", StringComparison.Ordinal))
    {
      throw new InvalidOperationException("Command already has a group by clause!");
    }

    var groupByBody = string.Join(", ", new Regex("new <>f__AnonymousType[0-9]{1,}`[0-9]{1,}", RegexOptions.None, TimeSpan.FromSeconds(3))
      .Replace(
        predicate.Body
          .ToString()
          .Replace($"{predicate.Parameters[0].Name}.", string.Empty, StringComparison.Ordinal), string.Empty)
      .Replace("(", string.Empty, StringComparison.Ordinal)
      .Replace(")", string.Empty, StringComparison.Ordinal)
      .Split(',')
      .Select(_ => _.Split("=")[0].Trim()));

    foreach (var property in this.Properties)
    {
      var propertyName = property.Value.Name;
      var propertyTableAlias = string.IsNullOrWhiteSpace(property.Value.Table)
        ? this.Table.Alias
        : this.Joins.Single(join => string.Equals(join.Table, property.Value.Table, StringComparison.Ordinal)).Alias;

      groupByBody = groupByBody.Replace($"{property.Key.Name}", $"{propertyTableAlias}.{propertyName}", StringComparison.Ordinal);
    }

    this.SqlBuilder.Append($" GROUP BY {groupByBody.Trim()}");

    return this;
  }

  /// <summary>
  /// Adds a group by clause.
  /// https://docs.snowflake.com/en/sql-reference/constructs/group-by .
  /// </summary>
  /// <param name="groupBy">The group by clause.</param>
  /// <returns>The snowflake command.</returns>
  /// <exception cref="InvalidOperationException">Command already has a group by clause.</exception>
  /// <exception cref="ArgumentNullException">Value for group by clause may not be empty.</exception>
  /// <exception cref="ArgumentException">Group By may only contain valid properties.</exception>
  public SnowflakeCommand<T> GroupBy(string groupBy)
  {
    if (this.Sql.Contains("GROUP BY", StringComparison.Ordinal))
    {
      throw new InvalidOperationException("Command already has a group by clause!");
    }

    if (string.IsNullOrWhiteSpace(groupBy))
    {
      throw new ArgumentNullException(nameof(groupBy), "Value for group by clause may not be empty!");
    }

    var regexTestValue = groupBy.Replace("GROUP BY", string.Empty, StringComparison.Ordinal).Trim();
    if (!this.ValidPropertiesRegex.IsMatch(regexTestValue))
    {
#pragma warning disable MA0015 // Specify the parameter name in ArgumentException
      throw new ArgumentException($"Group By may only contain valid properties ({string.Join(", ", this.ValidProperties)})! \"{regexTestValue}\" is not allowed!");
#pragma warning restore MA0015 // Specify the parameter name in ArgumentException
    }

    this.SqlBuilder.Append($" {(groupBy.Trim().StartsWith("GROUP BY", ignoreCase: true, CultureInfo.InvariantCulture) ? groupBy.Trim() : $"GROUP BY {groupBy.Trim()}")}");

    return this;
  }
}
