//-----------------------------------------------------------------------
// <copyright file="SnowflakeCommand.SingleOrDefault.cs" company="Jonas Schubert">
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
    /// Gets the single item for a query or null if none is found.
    /// </summary>
    /// <param name="parameterList">The parameter list.</param>
    /// <returns>The single item if found, otherwise null.</returns>
    /// <exception cref="InvalidOperationException">Result has more then one element.</exception>
    public T? SingleOrDefault(IList<(string, DbType, object)>? parameterList = default)
    {
      if (this._snowflakeDbConnection is null)
      {
        using var dbConnection = new SnowflakeDbConnection
        {
          ConnectionString = EnvironmentExtensions.GetSnowflakeConnectionString(),
        };
        dbConnection.Open();

        var item = this.SingleOrDefault(dbConnection, parameterList);

        dbConnection.Close();

        return item;
      }

      if (!this._snowflakeDbConnection.IsOpen())
      {
        this._snowflakeDbConnection.Open();
      }

      return this.SingleOrDefault(this._snowflakeDbConnection, parameterList);
    }

    private T? SingleOrDefault(SnowflakeDbConnection snowflakeDbConnection, IList<(string, DbType, object)>? parameterList = default)
    {
      var command = snowflakeDbConnection.CreateCommand();
      command.CommandText = this.Sql;

      var totalParameterList = this.ParameterList.Concat(parameterList ?? []).ToList();
      foreach (var parameter in totalParameterList)
      {
        command.AddParameter(parameter.Item1, parameter.Item2, parameter.Item3);
      }

      var reader = command.ExecuteReader();
      var item = reader.ToList<T>().SingleOrDefault();

      return item;
    }
  }
}
