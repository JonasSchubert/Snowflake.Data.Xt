//-----------------------------------------------------------------------
// <copyright file="SnowflakeCommand.ToListAsync.cs" company="Jonas Schubert">
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
  /// Gets a list of items for a query.
  /// </summary>
  /// <param name="parameterList">The parameter list.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>A list of items.</returns>
  public async Task<IList<T>> ToListAsync(IList<(string, DbType, object)>? parameterList = default, CancellationToken cancellationToken = default)
  {
    if (this.snowflakeDbConnection is null)
    {
      using var dbConnection = new SnowflakeDbConnection
      {
        ConnectionString = EnvironmentExtensions.GetSnowflakeConnectionString(),
      };
      await dbConnection.OpenAsync(cancellationToken).ConfigureAwait(false);

      var list = await this.ToListAsync(dbConnection, parameterList, cancellationToken).ConfigureAwait(false);

      await dbConnection.CloseAsync(cancellationToken).ConfigureAwait(false);

      return list;
    }

    if (!this.snowflakeDbConnection.IsOpen())
    {
      await this.snowflakeDbConnection.OpenAsync(cancellationToken).ConfigureAwait(false);
    }

    return await this.ToListAsync(this.snowflakeDbConnection, parameterList, cancellationToken).ConfigureAwait(false);
  }

  private async Task<IList<T>> ToListAsync(SnowflakeDbConnection snowflakeDbConnection, IList<(string, DbType, object)>? parameterList = default, CancellationToken cancellationToken = default)
  {
    this.WriteLogInformation("Performing snowflake command to retrieve a list of entities (ToListAsync).");

    var command = snowflakeDbConnection.CreateCommand();
    this.WriteLogInformation(this.Sql);
    command.CommandText = this.Sql;

    var totalParameterList = this.ParameterList.Concat(parameterList ?? new List<(string, DbType, object)>()).ToList();
    this.WriteLogInformation(string.Format(CultureInfo.InvariantCulture, "Adding {0} parameters.", totalParameterList.Count));
    foreach (var parameter in totalParameterList)
    {
      command.AddParameter(parameter.Item1, parameter.Item2, parameter.Item3);
    }

    var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
    var list = reader.ToList<T>();

    this.WriteLogInformation($"Found {list.Count} items for the provided query.");

    return list;
  }
}
