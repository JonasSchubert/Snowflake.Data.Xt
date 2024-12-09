//-----------------------------------------------------------------------
// <copyright file="SnowflakeCommand.Select.cs" company="Jonas Schubert">
//     Copyright (c) Jonas Schubert. All rights reserved.
// </copyright>
// <author>Jonas Schubert</author>
//-----------------------------------------------------------------------

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
    /// Adds a select filter to only query for limited columns.
    /// https://docs.snowflake.com/en/sql-reference/sql/select .
    /// </summary>
    /// <typeparam name="TSelect">The generic type.</typeparam>
    /// <param name="predicate">The select predicate.</param>
    /// <returns>The snowflake command.</returns>
    public SnowflakeCommand<T> Select<TSelect>(Expression<Func<T, TSelect>> predicate)
    {
      var selectBody = string.Join(", ", new Regex("new <>f__AnonymousType[0-9]{1,}`[0-9]{1,}", RegexOptions.None, TimeSpan.FromSeconds(3))
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

        selectBody = selectBody.Replace($"{property.Key.Name}", $"{propertyTableAlias}.{propertyName}", StringComparison.Ordinal);
      }

      this.SqlBuilder.Replace(this.Columns, selectBody.Trim());

      return this;
    }
  }
}
