//-----------------------------------------------------------------------
// <copyright file="SnowflakeTableAttribute.cs" company="Jonas Schubert">
//     Copyright (c) Jonas Schubert. All rights reserved.
// </copyright>
// <author>EWP Team Fürth</author>
//-----------------------------------------------------------------------

namespace Snowflake.Data.Xt;

/// <summary>
/// The snowflake table attribute.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class SnowflakeTableAttribute : Attribute
{
  /// <summary>
  /// Initializes a new instance of the <see cref="SnowflakeTableAttribute"/> class.
  /// </summary>
  /// <param name="name">The name.</param>
  /// <param name="alias">The alias.</param>
  public SnowflakeTableAttribute(string? name = default, string? alias = default)
  {
    this.Name = name ?? string.Empty;
    this.Alias = alias;
  }

  /// <summary>
  /// Gets the alias.
  /// </summary>
  public string? Alias { get; internal set; }

  /// <summary>
  /// Gets the name.
  /// </summary>
  public string Name { get; internal set; }
}
