//-----------------------------------------------------------------------
// <copyright file="SnowflakeCommand.ToList.cs" company="Jonas Schubert">
//     Copyright (c) Jonas Schubert. All rights reserved.
// </copyright>
// <author>EWP Team Fürth</author>
//-----------------------------------------------------------------------

using System.Globalization;
using Snowflake.Data.Client;

namespace Snowflake.Data.Xt;

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
    this.WriteLogInformation("Performing snowflake command to retrieve a list of entities (ToList).");

    using var dbConnection = new SnowflakeDbConnection
    {
      ConnectionString = EnvironmentExtensions.GetSnowflakeConnectionString(),
    };
    dbConnection.Open();

    var command = dbConnection.CreateCommand();
    this.WriteLogInformation(this.Sql);
    command.CommandText = this.Sql;

    var totalParameterList = this.ParameterList.Concat(parameterList ?? new List<(string, DbType, object)>()).ToList();
    this.WriteLogInformation(string.Format(CultureInfo.InvariantCulture, "Adding {0} parameters.", totalParameterList.Count));
    foreach (var parameter in totalParameterList)
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
