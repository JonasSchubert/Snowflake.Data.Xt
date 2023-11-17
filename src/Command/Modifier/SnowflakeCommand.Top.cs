namespace Snowflake.Data.Xt;

public partial class SnowflakeCommand<T>
  where T : class
{
  /// <summary>
  /// Sets a maximum amount of rows to return.
  /// https://docs.snowflake.com/en/sql-reference/constructs/top_n
  /// </summary>
  /// <param name="amount">The amount.</param>
  /// <returns>The snowflake command.</returns>
  /// <exception cref="InvalidOperationException">Command is already marked using top amount!</exception>
  /// <exception cref="ArgumentOutOfRangeException">Amount must be a positive integer!</exception>
  public SnowflakeCommand<T> Top(int amount)
  {
    if (this.Sql.Contains("SELECT DISTINCT TOP") || this.Sql.Contains("SELECT TOP"))
    {
      throw new InvalidOperationException("Command is already marked using top amount!");
    }

    if (amount <= 0)
    {
      throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be a positive integer!");
    }

    if (this.Sql.StartsWith("SELECT DISTINCT", StringComparison.InvariantCulture))
    {
      this.SqlBuilder.Replace("SELECT DISTINCT", $"SELECT DISTINCT TOP {amount}");
    }
    else
    {
      this.SqlBuilder.Replace("SELECT", $"SELECT TOP {amount}");
    }

    return this;
  }
}
