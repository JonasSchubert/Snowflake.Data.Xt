//-----------------------------------------------------------------------
// <copyright file="SnowflakeCommand.Having.cs" company="Jonas Schubert">
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
    /// Adds a having value. Requires a group by clause.
    /// https://docs.snowflake.com/en/sql-reference/constructs/having .
    /// </summary>
    /// <param name="having">The having value.</param>
    /// <returns>The snowflake command.</returns>
    /// <exception cref="InvalidOperationException">Command already has a having clause.</exception>
    /// <exception cref="InvalidOperationException">Group By is missing and has to be called before Having.</exception>
    /// <exception cref="ArgumentNullException">Value for having predicate may not be empty.</exception>
    public SnowflakeCommand<T> Having(string having)
    {
      if (this.Sql.Contains("HAVING", StringComparison.Ordinal))
      {
        throw new InvalidOperationException("Command already has a having clause!");
      }

      if (!this.Sql.Contains("GROUP BY", StringComparison.Ordinal))
      {
        throw new InvalidOperationException("Group By is missing and has to be called before Having!");
      }

      if (string.IsNullOrWhiteSpace(having))
      {
        throw new ArgumentNullException(nameof(having), "Value for having predicate may not be empty!");
      }

      this.SqlBuilder.Append($" {(having.Trim().StartsWith("HAVING", ignoreCase: true, CultureInfo.InvariantCulture) ? having.Trim() : $"HAVING {having.Trim()}")}");

      return this;
    }
  }
}
