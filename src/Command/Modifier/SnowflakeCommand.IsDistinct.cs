//-----------------------------------------------------------------------
// <copyright file="SnowflakeCommand.IsDistinct.cs" company="Jonas Schubert">
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
    /// Ensures a distinct query.
    /// </summary>
    /// <returns>The snowflake command.</returns>
    /// <exception cref="InvalidOperationException">Command already marked as distinct.</exception>
    public SnowflakeCommand<T> IsDistinct()
    {
      if (this.Sql.Contains("SELECT DISTINCT", StringComparison.Ordinal))
      {
        throw new InvalidOperationException("Command is already marked as distinct!");
      }

      this.SqlBuilder.Replace("SELECT", "SELECT DISTINCT");

      return this;
    }
  }
}
