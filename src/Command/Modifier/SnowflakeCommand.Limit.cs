//-----------------------------------------------------------------------
// <copyright file="SnowflakeCommand.Limit.cs" company="Jonas Schubert">
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
    /// Constrains the maximum number of rows returned by a statement or subquery.
    /// https://docs.snowflake.com/en/sql-reference/constructs/limit .
    /// </summary>
    /// <param name="count">The count.</param>
    /// <param name="offset">The offset.</param>
    /// <returns>The snowflake command.</returns>
    /// <exception cref="InvalidOperationException">Command is already marked using limit.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Count must be a positive integer.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Offset must be a positive integer.</exception>
    public SnowflakeCommand<T> Limit(int count, int offset = 0)
    {
      if (this.Sql.Contains(" LIMIT ", StringComparison.Ordinal))
      {
        throw new InvalidOperationException("Command is already marked using limit!");
      }

      if (count < 0)
      {
        throw new ArgumentOutOfRangeException(nameof(count), "Count must be 0 or a positive integer!");
      }

      if (offset < 0)
      {
        throw new ArgumentOutOfRangeException(nameof(offset), "Offset must be 0 or a positive integer!");
      }

      this.SqlBuilder.Append($" LIMIT {count} OFFSET {offset}");

      return this;
    }
  }
}
