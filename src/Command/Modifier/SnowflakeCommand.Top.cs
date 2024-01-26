//-----------------------------------------------------------------------
// <copyright file="SnowflakeCommand.Top.cs" company="Jonas Schubert">
//     Copyright (c) Jonas Schubert. All rights reserved.
// </copyright>
// <author>Jonas Schubert</author>
//-----------------------------------------------------------------------

namespace Snowflake.Data.Xt;

/// <summary>
/// The snowflake command.
/// </summary>
/// <typeparam name="T">The generic type. This is used to parse properties for the query.</typeparam>
public partial class SnowflakeCommand<T>
  where T : class
{
  /// <summary>
  /// Sets a maximum amount of rows to return.
  /// https://docs.snowflake.com/en/sql-reference/constructs/top_n .
  /// </summary>
  /// <param name="amount">The amount.</param>
  /// <returns>The snowflake command.</returns>
  /// <exception cref="InvalidOperationException">Command is already marked using top amount.</exception>
  /// <exception cref="ArgumentOutOfRangeException">Amount must be a positive integer.</exception>
  public SnowflakeCommand<T> Top(int amount)
  {
    if (this.Sql.Contains("SELECT DISTINCT TOP", StringComparison.Ordinal) || this.Sql.Contains("SELECT TOP", StringComparison.Ordinal))
    {
      throw new InvalidOperationException("Command is already marked using top amount!");
    }

    if (amount <= 0)
    {
      throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be a positive integer!");
    }

    if (this.Sql.StartsWith("SELECT DISTINCT", StringComparison.InvariantCulture))
    {
      this.SqlBuilder.Replace("SELECT DISTINCT", string.Format(CultureInfo.InvariantCulture, "SELECT DISTINCT TOP {0}", amount));
    }
    else
    {
      this.SqlBuilder.Replace("SELECT", string.Format(CultureInfo.InvariantCulture, "SELECT TOP {0}", amount));
    }

    return this;
  }
}
