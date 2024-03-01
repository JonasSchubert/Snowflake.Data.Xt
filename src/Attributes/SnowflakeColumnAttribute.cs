//-----------------------------------------------------------------------
// <copyright file="SnowflakeColumnAttribute.cs" company="Jonas Schubert">
//     Copyright (c) Jonas Schubert. All rights reserved.
// </copyright>
// <author>Jonas Schubert</author>
//-----------------------------------------------------------------------

namespace Snowflake.Data.Xt;

/// <summary>
/// The snowflake column attribute.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class SnowflakeColumnAttribute : Attribute
{
  /// <summary>
  /// Initializes a new instance of the <see cref="SnowflakeColumnAttribute"/> class.
  /// </summary>
  /// <param name="name">The name.</param>
  /// <param name="table">The table.</param>
  /// <param name="alias">The alias.</param>
  public SnowflakeColumnAttribute(string? name = default, string? table = default, string? alias = default)
  {
    this.Alias = alias;
    this.Name = name ?? string.Empty;
    this.Table = table;
  }

  /// <summary>
  /// Gets the alias.
  /// </summary>
  public string? Alias { get; internal set; }

  /// <summary>
  /// Gets the name.
  /// </summary>
  public string Name { get; internal set; }

  /// <summary>
  /// Gets the table.
  /// </summary>
  public string? Table { get; }
}
