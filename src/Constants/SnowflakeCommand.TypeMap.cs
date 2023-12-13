//-----------------------------------------------------------------------
// <copyright file="SnowflakeCommand.TypeMap.cs" company="Jonas Schubert">
//     Copyright (c) Jonas Schubert. All rights reserved.
// </copyright>
// <author>EWP Team Fürth</author>
//-----------------------------------------------------------------------

namespace Snowflake.Data.Xt;

/// <summary>
/// The snowflake command.
/// </summary>
/// <typeparam name="T">The generic type. This is used to parse properties for the query.</typeparam>
public partial class SnowflakeCommand<T>
  where T : class
{
  /// <summary>
  /// The type map.
  /// https://stackoverflow.com/a/7952171 .
  /// </summary>
  /// <returns>A dictionary with maps for Type to DbType.</returns>
  private readonly IDictionary<Type, DbType> typeMap = new Dictionary<Type, DbType>()
  {
    { typeof(byte),  DbType.Byte },
    { typeof(sbyte),  DbType.SByte },
    { typeof(short),  DbType.Int16 },
    { typeof(ushort),  DbType.UInt16 },
    { typeof(int),  DbType.Int32 },
    { typeof(uint),  DbType.UInt32 },
    { typeof(long),  DbType.Int64 },
    { typeof(ulong),  DbType.UInt64 },
    { typeof(float),  DbType.Single },
    { typeof(double),  DbType.Double },
    { typeof(decimal),  DbType.Decimal },
    { typeof(bool),  DbType.Boolean },
    { typeof(string),  DbType.String },
    { typeof(char),  DbType.StringFixedLength },
    { typeof(Guid),  DbType.Guid },
    { typeof(DateTime),  DbType.DateTime },
    { typeof(DateTimeOffset),  DbType.DateTimeOffset },
    { typeof(byte[]),  DbType.Binary },
    { typeof(byte?),  DbType.Byte },
    { typeof(sbyte?),  DbType.SByte },
    { typeof(short?),  DbType.Int16 },
    { typeof(ushort?),  DbType.UInt16 },
    { typeof(int?),  DbType.Int32 },
    { typeof(uint?),  DbType.UInt32 },
    { typeof(long?),  DbType.Int64 },
    { typeof(ulong?),  DbType.UInt64 },
    { typeof(float?),  DbType.Single },
    { typeof(double?),  DbType.Double },
    { typeof(decimal?),  DbType.Decimal },
    { typeof(bool?),  DbType.Boolean },
    { typeof(char?),  DbType.StringFixedLength },
    { typeof(Guid?),  DbType.Guid },
    { typeof(DateTime?),  DbType.DateTime },
    { typeof(DateTimeOffset?),  DbType.DateTimeOffset },
  };
}
