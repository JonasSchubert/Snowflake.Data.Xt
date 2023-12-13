//-----------------------------------------------------------------------
// <copyright file="SnowflakeCommand.FirstOrDefault.cs" company="Jonas Schubert">
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
  /// Gets the first item for a query or null if none is found.
  /// </summary>
  /// <param name="parameterList">The parameter list.</param>
  /// <returns>The first item if found, otherwise null.</returns>
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

    this.WriteLogInformation(string.Format(CultureInfo.InvariantCulture, "Adding {0} parameters.", parameterList?.Count ?? 0));
    foreach (var parameter in parameterList ?? new List<(string, DbType, object)>())
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
