//-----------------------------------------------------------------------
// <copyright file="SnowflakeCommand.ToList.cs" company="Jonas Schubert">
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
    /// Gets a list of items for a query.
    /// </summary>
    /// <param name="parameterList">The parameter list.</param>
    /// <returns>A list of items.</returns>
    public IList<T> ToList(IList<(string, DbType, object)>? parameterList = default)
    {
      if (this._snowflakeDbConnection is null)
      {
        using var dbConnection = new SnowflakeDbConnection
        {
          ConnectionString = EnvironmentExtensions.GetSnowflakeConnectionString(),
        };
        dbConnection.Open();

        var list = this.ToList(dbConnection, parameterList);

        dbConnection.Close();

        return list;
      }

      if (!this._snowflakeDbConnection.IsOpen())
      {
        this._snowflakeDbConnection.Open();
      }

      return this.ToList(this._snowflakeDbConnection, parameterList);
    }

    private IList<T> ToList(SnowflakeDbConnection snowflakeDbConnection, IList<(string, DbType, object)>? parameterList = default)
    {
      this.WriteLogInformation("Performing snowflake command to retrieve a list of entities (ToList).");

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
      var list = reader.ToList<T>();

      this.WriteLogInformation($"Found {list.Count} items for the provided query.");

      return list;
    }
  }
}
