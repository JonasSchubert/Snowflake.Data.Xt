//-----------------------------------------------------------------------
// <copyright file="SnowflakeCommand.FirstOrDefaultAsync.cs" company="Jonas Schubert">
//     Copyright (c) Jonas Schubert. All rights reserved.
// </copyright>
// <author>Jonas Schubert</author>
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
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The first item if found, otherwise null.</returns>
  public async Task<T?> FirstOrDefaultAsync(IList<(string, DbType, object)>? parameterList = default, CancellationToken cancellationToken = default)
  {
    this.WriteLogInformation("Performing snowflake command to retrieve one entity (FirstOrDefaultAsync).");

    using var dbConnection = new SnowflakeDbConnection
    {
      ConnectionString = EnvironmentExtensions.GetSnowflakeConnectionString(),
    };
    await dbConnection.OpenAsync(cancellationToken).ConfigureAwait(false);

    var command = dbConnection.CreateCommand();
    this.WriteLogInformation(this.Sql);
    command.CommandText = this.Sql;

    var totalParameterList = this.ParameterList.Concat(parameterList ?? new List<(string, DbType, object)>()).ToList();
    this.WriteLogInformation(string.Format(CultureInfo.InvariantCulture, "Adding {0} parameters.", totalParameterList.Count));
    foreach (var parameter in totalParameterList)
    {
      command.AddParameter(parameter.Item1, parameter.Item2, parameter.Item3);
    }

    var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
    var item = reader.FirstOrDefault<T>();

    this.WriteLogInformation($"Found{(item is not null ? string.Empty : " no")} item for the provided query.");

    await dbConnection.CloseAsync(cancellationToken).ConfigureAwait(false);

    return item;
  }
}
