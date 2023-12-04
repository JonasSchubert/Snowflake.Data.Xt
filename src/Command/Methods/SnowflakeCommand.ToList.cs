using Snowflake.Data.Client;

namespace Snowflake.Data.Xt;
public partial class SnowflakeCommand<T>
  where T : class
{
  public IList<T> ToList(IList<(string, DbType, object)>? parameterList = default)
  {
    this.WriteLogInformation("Performing snowflake command to retrieve a list of entities (ToList).");

    using var dbConnection = new SnowflakeDbConnection
    {
      ConnectionString = EnvironmentExtensions.GetSnowflakeConnectionString(),
    };
    dbConnection.Open();

    var command = dbConnection.CreateCommand();
    this.WriteLogInformation(this.Sql);
    command.CommandText = this.Sql;

    this.WriteLogInformation($"Adding {parameterList?.Count ?? 0} parameters.");
    foreach (var parameter in parameterList ?? new List<(string, DbType, object)>())
    {
      command.AddParameter(parameter.Item1, parameter.Item2, parameter.Item3);
    }

    var reader = command.ExecuteReader();
    var list = reader.ToList<T>();

    this.WriteLogInformation($"Found {list.Count} items for the provided query.");

    dbConnection.Close();

    return list;
  }
}
