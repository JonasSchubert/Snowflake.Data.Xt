//-----------------------------------------------------------------------
// <copyright file="DbDataReaderExtensions.cs" company="Jonas Schubert">
//     Copyright (c) Jonas Schubert. All rights reserved.
// </copyright>
// <author>Jonas Schubert</author>
//-----------------------------------------------------------------------

using System.Data.Common;
using System.Reflection;

namespace Snowflake.Data.Xt
{
  /// <summary>
  /// The static database data reader extensions class.
  /// </summary>
  public static class DbDataReaderExtensions
  {
    /// <summary>
    /// Gets the first item for a query or null if none is found.
    /// </summary>
    /// <typeparam name="T">The generic type.</typeparam>
    /// <param name="dbDataReader">The database data reader.</param>
    /// <returns>The first item if found, otherwise null.</returns>
    public static T? FirstOrDefault<T>(this DbDataReader dbDataReader)
      where T : class
    {
      if (dbDataReader.Read())
      {
        T item = (T)Expression.Lambda<Func<object>>(
          Expression.Convert(Expression.New(typeof(T)), typeof(object)))
          .Compile()
          .Invoke();

        foreach (var propertyInfo in typeof(T).GetProperties().Where(property => Attribute.IsDefined(property, typeof(SnowflakeColumnAttribute))))
        {
          var property = item.GetType().GetProperty(propertyInfo.Name);
          if (property is null || !property.CanWrite)
          {
            continue;
          }

          SnowflakeColumnAttribute? snowflakeColumn = (SnowflakeColumnAttribute?)propertyInfo.GetCustomAttributes(typeof(SnowflakeColumnAttribute), inherit: true).SingleOrDefault();
          if (snowflakeColumn is null)
          {
            continue;
          }

          if (string.IsNullOrWhiteSpace(snowflakeColumn.Name))
          {
            snowflakeColumn.Name = propertyInfo.Name;
          }

          DbDataReaderExtensions.SetPropertyValue(property, item, dbDataReader, snowflakeColumn);
        }

        return item;
      }

      return null;
    }

    /// <summary>
    /// Gets a list of items for a query.
    /// </summary>
    /// <typeparam name="T">The generic type.</typeparam>
    /// <param name="dbDataReader">The database data reader.</param>
    /// <returns>A list of items.</returns>
    public static IList<T> ToList<T>(this DbDataReader dbDataReader)
      where T : class
    {
      var list = new List<T>();

      T? item;
      do
      {
        item = dbDataReader.FirstOrDefault<T>();
        if (item is not null)
        {
          list.Add(item);
        }
      }
      while (item is not null);

      return list;
    }

    /// <summary>
    /// Sets a property value using snowflake column attribute and property info.
    /// </summary>
    /// <typeparam name="T">The generic type.</typeparam>
    /// <param name="property">The property.</param>
    /// <param name="item">The item.</param>
    /// <param name="dbDataReader">The database data reader.</param>
    /// <param name="snowflakeColumn">The snowflake column.</param>
    /// <exception cref="NotSupportedException">If a property type is not supported, a NotSupportedException will be thrown.</exception>
    private static void SetPropertyValue<T>(PropertyInfo property, T item, DbDataReader dbDataReader, SnowflakeColumnAttribute snowflakeColumn)
    {
      if (property.PropertyType == typeof(string))
      {
        property.SetValue(item, dbDataReader.IsDBNull(snowflakeColumn.ColumnAlias ?? snowflakeColumn.Name) ? null : dbDataReader.GetString(snowflakeColumn.ColumnAlias ?? snowflakeColumn.Name));
      }
      else if (property.PropertyType == typeof(decimal))
      {
        property.SetValue(item, dbDataReader.GetDecimal(snowflakeColumn.ColumnAlias ?? snowflakeColumn.Name));
      }
      else if (property.PropertyType == typeof(decimal?))
      {
        property.SetValue(item, dbDataReader.IsDBNull(snowflakeColumn.ColumnAlias ?? snowflakeColumn.Name) ? (decimal?)null : dbDataReader.GetDecimal(snowflakeColumn.ColumnAlias ?? snowflakeColumn.Name));
      }
      else if (property.PropertyType == typeof(double))
      {
        property.SetValue(item, dbDataReader.GetDouble(snowflakeColumn.ColumnAlias ?? snowflakeColumn.Name));
      }
      else if (property.PropertyType == typeof(double?))
      {
        property.SetValue(item, dbDataReader.IsDBNull(snowflakeColumn.ColumnAlias ?? snowflakeColumn.Name) ? (double?)null : dbDataReader.GetDouble(snowflakeColumn.ColumnAlias ?? snowflakeColumn.Name));
      }
      else if (property.PropertyType == typeof(float))
      {
        property.SetValue(item, dbDataReader.GetFloat(snowflakeColumn.ColumnAlias ?? snowflakeColumn.Name));
      }
      else if (property.PropertyType == typeof(float?))
      {
        property.SetValue(item, dbDataReader.IsDBNull(snowflakeColumn.ColumnAlias ?? snowflakeColumn.Name) ? (float?)null : dbDataReader.GetFloat(snowflakeColumn.ColumnAlias ?? snowflakeColumn.Name));
      }
      else if (property.PropertyType == typeof(int))
      {
        property.SetValue(item, dbDataReader.GetInt32(snowflakeColumn.ColumnAlias ?? snowflakeColumn.Name));
      }
      else if (property.PropertyType == typeof(int?))
      {
        property.SetValue(item, dbDataReader.IsDBNull(snowflakeColumn.ColumnAlias ?? snowflakeColumn.Name) ? (int?)null : dbDataReader.GetInt32(snowflakeColumn.ColumnAlias ?? snowflakeColumn.Name));
      }
      else if (property.PropertyType == typeof(bool))
      {
        property.SetValue(item, dbDataReader.GetBoolean(snowflakeColumn.ColumnAlias ?? snowflakeColumn.Name));
      }
      else if (property.PropertyType == typeof(bool?))
      {
        property.SetValue(item, dbDataReader.IsDBNull(snowflakeColumn.ColumnAlias ?? snowflakeColumn.Name) ? (bool?)null : dbDataReader.GetBoolean(snowflakeColumn.ColumnAlias ?? snowflakeColumn.Name));
      }
      else if (property.PropertyType == typeof(byte))
      {
        property.SetValue(item, dbDataReader.GetByte(snowflakeColumn.ColumnAlias ?? snowflakeColumn.Name));
      }
      else if (property.PropertyType == typeof(byte?))
      {
        property.SetValue(item, dbDataReader.IsDBNull(snowflakeColumn.ColumnAlias ?? snowflakeColumn.Name) ? (byte?)null : dbDataReader.GetByte(snowflakeColumn.ColumnAlias ?? snowflakeColumn.Name));
      }
      else if (property.PropertyType == typeof(DateTime))
      {
        property.SetValue(item, dbDataReader.GetDateTime(snowflakeColumn.ColumnAlias ?? snowflakeColumn.Name));
      }
      else if (property.PropertyType == typeof(DateTime?))
      {
        property.SetValue(item, dbDataReader.IsDBNull(snowflakeColumn.ColumnAlias ?? snowflakeColumn.Name) ? (DateTime?)null : dbDataReader.GetDateTime(snowflakeColumn.ColumnAlias ?? snowflakeColumn.Name));
      }
      else
      {
        throw new NotSupportedException($"Type '{property.PropertyType}' is not yet supported. Please create an issue. Thank you!");
      }
    }
  }
}
