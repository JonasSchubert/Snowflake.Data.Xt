//-----------------------------------------------------------------------
// <copyright file="DbCommandExtensions.cs" company="Jonas Schubert">
//     Copyright (c) Jonas Schubert. All rights reserved.
// </copyright>
// <author>EWP Team Fürth</author>
//-----------------------------------------------------------------------

using System.Data.Common;

namespace Snowflake.Data.Xt;

/// <summary>
/// The static database command extensions class.
/// </summary>
public static class DbCommandExtensions
{
  /// <summary>
  /// Adds a parameter to the database command.
  /// </summary>
  /// <param name="dbCommand">The database command.</param>
  /// <param name="parameterName">The parameter name.</param>
  /// <param name="dbType">The database type.</param>
  /// <param name="value">The value.</param>
  public static void AddParameter(this DbCommand dbCommand, string parameterName, DbType dbType, object value)
  {
    var parameter = dbCommand.CreateParameter();

    parameter.ParameterName = parameterName;
    parameter.DbType = dbType;
    parameter.Value = value;

    dbCommand.Parameters.Add(parameter);
  }
}
