//-----------------------------------------------------------------------
// <copyright file="SnowflakeCommand.OrderBy.cs" company="Jonas Schubert">
//     Copyright (c) Jonas Schubert. All rights reserved.
// </copyright>
// <author>Jonas Schubert</author>
//-----------------------------------------------------------------------

using TimeSpanXt;

namespace Snowflake.Data.Xt
{
  /// <summary>
  /// The snowflake command.
  /// </summary>
  /// <typeparam name="T">The generic type. This is used to parse properties for the query.</typeparam>
  public partial class SnowflakeCommand<T>
    where T : class
  {
    /// <summary>
    /// Adds an order by ascending clause.
    /// https://docs.snowflake.com/en/sql-reference/constructs/order-by .
    /// </summary>
    /// <typeparam name="TOrderBy">The generic type.</typeparam>
    /// <param name="predicate">The order by predicate.</param>
    /// <returns>The snowflake command.</returns>
    /// <exception cref="InvalidOperationException">Command already has an order by clause.</exception>
    public SnowflakeCommand<T> OrderByAsc<TOrderBy>(Expression<Func<T, TOrderBy>> predicate)
    {
      return this.OrderBy(predicate, OrderByDirection.ASC);
    }

    /// <summary>
    /// Adds an order by descending clause.
    /// https://docs.snowflake.com/en/sql-reference/constructs/order-by .
    /// </summary>
    /// <typeparam name="TOrderBy">The generic type.</typeparam>
    /// <param name="predicate">The order by predicate.</param>
    /// <returns>The snowflake command.</returns>
    /// <exception cref="InvalidOperationException">Command already has an order by clause.</exception>
    public SnowflakeCommand<T> OrderByDesc<TOrderBy>(Expression<Func<T, TOrderBy>> predicate)
    {
      return this.OrderBy(predicate, OrderByDirection.DESC);
    }

    /// <summary>
    /// Adds a order by clause.
    /// https://docs.snowflake.com/en/sql-reference/constructs/order-by .
    /// </summary>
    /// <param name="orderBy">The order by clause.</param>
    /// <returns>The snowflake command.</returns>
    /// <exception cref="InvalidOperationException">Command already has a order by clause.</exception>
    /// <exception cref="ArgumentNullException">Value for order by clause may not be empty.</exception>
    /// <exception cref="ArgumentException">Order By may only contain valid properties.</exception>
    [Obsolete("Use OrderBy<TOrderBy>(Expression<Func<T, TOrderBy>> predicate, OrderByDirection direction, NullsHandling nullsHandling) instead.")]
    public SnowflakeCommand<T> OrderBy(string orderBy)
    {
      if (this.Sql.Contains("ORDER BY", StringComparison.Ordinal))
      {
        throw new InvalidOperationException("Command already has an order by clause!");
      }

      if (string.IsNullOrWhiteSpace(orderBy))
      {
        throw new ArgumentNullException(nameof(orderBy), "Value for order by clause may not be empty!");
      }

      var regexTestValue = orderBy
        .Replace("ORDER BY", string.Empty, StringComparison.Ordinal)
        .Replace("ASC", string.Empty, StringComparison.Ordinal)
        .Replace("DESC", string.Empty, StringComparison.Ordinal)
        .Trim();
      if (!this.ValidPropertiesRegex.IsMatch(regexTestValue))
      {
        throw new ArgumentException($"Order By may only contain valid properties ({string.Join(", ", this.ValidProperties)})! \"{regexTestValue}\" is not allowed!");
      }

      this.SqlBuilder.Append($" {(orderBy.Trim().StartsWith("ORDER BY", ignoreCase: true, CultureInfo.InvariantCulture) ? orderBy.Trim() : $"ORDER BY {orderBy.Trim()}")}");

      return this;
    }

    /// <summary>
    /// Adds an order by clause.
    /// https://docs.snowflake.com/en/sql-reference/constructs/order-by .
    /// </summary>
    /// <typeparam name="TOrderBy">The generic type.</typeparam>
    /// <param name="predicate">The order by predicate.</param>
    /// <param name="direction">The direction. ASC or DESC.</param>
    /// <returns>The snowflake command.</returns>
    /// <exception cref="InvalidOperationException">Command already has an order by clause.</exception>
    /// <exception cref="InvalidOperationException">Direction is not a valid value.</exception>
    public SnowflakeCommand<T> OrderBy<TOrderBy>(Expression<Func<T, TOrderBy>> predicate, OrderByDirection direction)
    {
      return this.OrderBy(predicate, direction, NullsHandling.DEFAULT);
    }

    /// <summary>
    /// Adds an order by clause.
    /// https://docs.snowflake.com/en/sql-reference/constructs/order-by .
    /// </summary>
    /// <typeparam name="TOrderBy">The generic type.</typeparam>
    /// <param name="predicate">The order by predicate.</param>
    /// <param name="direction">The direction. ASC or DESC.</param>
    /// <param name="nullsHandling">The handling of nulls.</param>
    /// <returns>The snowflake command.</returns>
    /// <exception cref="InvalidOperationException">Command already has an order by clause.</exception>
    /// <exception cref="InvalidOperationException">Direction is not a valid value.</exception>
    public SnowflakeCommand<T> OrderBy<TOrderBy>(Expression<Func<T, TOrderBy>> predicate, OrderByDirection direction, NullsHandling nullsHandling)
    {
      if (this.Sql.Contains("ORDER BY", StringComparison.Ordinal))
      {
        throw new InvalidOperationException("Command already has an order by clause!");
      }

      var orderByBody = string.Join(", ", new Regex("new <>f__AnonymousType[0-9]{1,}`[0-9]{1,}", RegexOptions.None, 3.Seconds())
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

        orderByBody = orderByBody.Replace($"{property.Key.Name}", $"{propertyTableAlias}.{propertyName}", StringComparison.Ordinal);
      }

      this.SqlBuilder.Append($" ORDER BY {orderByBody.Trim()} {(direction is OrderByDirection.ASC ? "ASC" : "DESC")}{(nullsHandling is not NullsHandling.DEFAULT ? $" NULLS {(nullsHandling is NullsHandling.FIRST ? "FIRST" : "LAST")}" : string.Empty)}");

      return this;
    }

    /// <summary>
    /// Adds an order by clause.
    /// https://docs.snowflake.com/en/sql-reference/constructs/order-by .
    /// </summary>
    /// <typeparam name="TOrderBy">The generic type.</typeparam>
    /// <param name="predicate">The order by predicate.</param>
    /// <param name="direction">The direction. ASC or DESC.</param>
    /// <returns>The snowflake command.</returns>
    /// <exception cref="InvalidOperationException">Command already has an order by clause.</exception>
    /// <exception cref="InvalidOperationException">Direction is not a valid value.</exception>
    [Obsolete("Use OrderBy<TOrderBy>(Expression<Func<T, TOrderBy>> predicate, OrderByDirection direction, NullsHandling nullsHandling) instead.")]
    public SnowflakeCommand<T> OrderBy<TOrderBy>(Expression<Func<T, TOrderBy>> predicate, string direction)
    {
      return direction is not "ASC" and not "DESC"
        ? throw new InvalidOperationException($"Invalid direction '{direction}'! Only ASC or DESC are supported")
        : this.OrderBy(predicate, direction is "ASC" ? OrderByDirection.ASC : OrderByDirection.DESC);
    }
  }

  /// <summary>
  /// The handling of nulls.
  /// </summary>
  public enum NullsHandling
  {
    /// <summary>
    /// Use default handling for NULLS.
    /// </summary>
    DEFAULT,

    /// <summary>
    /// Force NULLS to be first.
    /// </summary>
    FIRST,

    /// <summary>
    /// Force NULLS to be last.
    /// </summary>
    LAST,
  }

  /// <summary>
  /// The order by direction.
  /// </summary>
  public enum OrderByDirection
  {
    /// <summary>
    /// Defines the ascending order.
    /// </summary>
    ASC,

    /// <summary>
    /// Defines the descending order.
    /// </summary>
    DESC,
  }
}
