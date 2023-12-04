using Snowflake.Data.Client;

namespace Snowflake.Data.Xt;
public partial class SnowflakeCommand<T>
  where T : class
{
  public async Task<IList<T>> ToListAsync(IList<(string, DbType, object)>? parameterList = default, CancellationToken cancellationToken = default)
  {
    this.WriteLogInformation("Performing snowflake command to retrieve a list of entities (ToListAsync).");

    using var dbConnection = new SnowflakeDbConnection
    {
      ConnectionString = EnvironmentExtensions.GetSnowflakeConnectionString(),
    };
    await dbConnection.OpenAsync(cancellationToken);

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

    await dbConnection.CloseAsync(cancellationToken);

    return list;
  }
}
