//-----------------------------------------------------------------------
// <copyright file="SnowflakeCommand.FirstOrDefault.cs" company="Jonas Schubert">
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
    /// Gets the first item for a query or null if none is found.
    /// </summary>
    /// <param name="parameterList">The parameter list.</param>
    /// <returns>The first item if found, otherwise null.</returns>
    public T? FirstOrDefault(IList<(string, DbType, object)>? parameterList = default)
    {
      if (this._snowflakeDbConnection is null)
      {
        using var dbConnection = new SnowflakeDbConnection
        {
          ConnectionString = EnvironmentExtensions.GetSnowflakeConnectionString(),
        };
        dbConnection.Open();

        var item = this.FirstOrDefault(dbConnection, parameterList);

        dbConnection.Close();

        return item;
      }

      if (!this._snowflakeDbConnection.IsOpen())
      {
        this._snowflakeDbConnection.Open();
      }

      return this.FirstOrDefault(this._snowflakeDbConnection, parameterList);
    }

    private T? FirstOrDefault(SnowflakeDbConnection snowflakeDbConnection, IList<(string, DbType, object)>? parameterList = default)
    {
      this.WriteLogInformation("Performing snowflake command to retrieve one entity (FirstOrDefault).");

      var command = snowflakeDbConnection.CreateCommand();
      this.WriteLogInformation(this.Sql);
      command.CommandText = this.Sql;

      var totalParameterList = this.ParameterList.Concat(parameterList ?? []).ToList();
      this.WriteLogInformation(string.Format(CultureInfo.InvariantCulture, "Adding {0} parameters.", totalParameterList.Count));
      foreach (var parameter in totalParameterList)
      {
        command.AddParameter(parameter.Item1, parameter.Item2, parameter.Item3);
      }

      var reader = command.ExecuteReader();
      var item = reader.FirstOrDefault<T>();

      this.WriteLogInformation($"Found{(item is not null ? string.Empty : " no")} item for the provided query.");

      return item;
    }
  }
}
