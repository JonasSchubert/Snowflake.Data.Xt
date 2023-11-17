namespace Snowflake.Data.Xt;

public partial class SnowflakeCommand<T>
  where T : class
{
  /// <summary>
  /// Ensures a distinct query.
  /// </summary>
  /// <returns>The snowflake command.</returns>
  /// <exception cref="InvalidOperationException">Command already marked as distinct!</exception>
  public SnowflakeCommand<T> IsDistinct()
  {
    if (this.Sql.Contains("SELECT DISTINCT"))
    {
      throw new InvalidOperationException("Command is already marked as distinct!");
    }

    this.SqlBuilder.Replace("SELECT", "SELECT DISTINCT");

    return this;
  }
}
