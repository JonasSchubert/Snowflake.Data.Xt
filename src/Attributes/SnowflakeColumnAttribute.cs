//-----------------------------------------------------------------------
// <copyright file="SnowflakeColumnAttribute.cs" company="Jonas Schubert">
//     Copyright (c) Jonas Schubert. All rights reserved.
// </copyright>
// <author>Jonas Schubert</author>
//-----------------------------------------------------------------------

namespace Snowflake.Data.Xt
{
  /// <summary>
  /// The snowflake column attribute.
  /// </summary>
  /// <remarks>
  /// Initializes a new instance of the <see cref="SnowflakeColumnAttribute"/> class.
  /// </remarks>
  /// <param name="name">The name.</param>
  /// <param name="table">The table.</param>
  /// <param name="tableAlias">The table alias.</param>
  /// <param name="columnAlias">The column alias.</param>
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
  public sealed class SnowflakeColumnAttribute(string? name = default, string? table = default, string? tableAlias = default, string? columnAlias = default) : Attribute
  {
    /// <summary>
    /// Gets the column alias.
    /// </summary>
    public string? ColumnAlias { get; internal set; } = columnAlias;

    /// <summary>
    /// Gets the name.
    /// </summary>
    public string Name { get; internal set; } = name ?? string.Empty;

    /// <summary>
    /// Gets the table.
    /// </summary>
    public string? Table { get; } = table;

    /// <summary>
    /// Gets the table alias.
    /// </summary>
    public string? TableAlias { get; internal set; } = tableAlias;
  }
}
