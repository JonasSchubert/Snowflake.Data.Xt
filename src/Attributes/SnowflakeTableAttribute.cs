//-----------------------------------------------------------------------
// <copyright file="SnowflakeTableAttribute.cs" company="Jonas Schubert">
//     Copyright (c) Jonas Schubert. All rights reserved.
// </copyright>
// <author>Jonas Schubert</author>
//-----------------------------------------------------------------------

namespace Snowflake.Data.Xt
{
  /// <summary>
  /// The snowflake table attribute.
  /// </summary>
  /// <remarks>
  /// Initializes a new instance of the <see cref="SnowflakeTableAttribute"/> class.
  /// </remarks>
  /// <param name="name">The name.</param>
  /// <param name="alias">The alias.</param>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public sealed class SnowflakeTableAttribute(string? name = default, string? alias = default) : Attribute
  {
    /// <summary>
    /// Gets the alias.
    /// </summary>
    public string? Alias { get; internal set; } = alias;

    /// <summary>
    /// Gets the name.
    /// </summary>
    public string Name { get; internal set; } = name ?? string.Empty;
  }
}
