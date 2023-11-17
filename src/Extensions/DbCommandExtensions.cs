using System.Data.Common;

namespace Snowflake.Data.Xt;

public static class DbCommandExtensions
{
  public static void AddParameter(this DbCommand dbCommand, string parameterName, DbType dbType, object value)
  {
    var parameter = dbCommand.CreateParameter();

    parameter.ParameterName = parameterName;
    parameter.DbType = dbType;
    parameter.Value = value;

    dbCommand.Parameters.Add(parameter);
  }
}
