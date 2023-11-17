using Snowflake.Data.Client;

namespace Snowflake.Data.Xt;

public partial class SnowflakeCommand<T>
  where T : class
{
  public T? FirstOrDefault(IList<(string, DbType, object)>? parameterList = default)
  {
    this.WriteLogInformation("Performing snowflake command to retrieve one entity (FirstOrDefault).");

    using var dbConnection = new SnowflakeDbConnection
    {
      ConnectionString = EnvironmentExtensions.GetSnowflakeConnectionString(),
    };
    dbConnection.Open();

    var command = dbConnection.CreateCommand();
    this.WriteLogInformation(this.Sql);
    command.CommandText = this.Sql;

    this.WriteLogInformation($"Adding {parameterList?.Count ?? 0} parameters.");
    foreach (var parameter in parameterList ?? [])
    {
      command.AddParameter(parameter.Item1, parameter.Item2, parameter.Item3);
    }

    var reader = command.ExecuteReader();
    var item = reader.FirstOrDefault<T>();

    this.WriteLogInformation($"Found{(item is not null ? string.Empty : " no")} item for the provided query.");

    dbConnection.Close();

    return item;
  }
}
