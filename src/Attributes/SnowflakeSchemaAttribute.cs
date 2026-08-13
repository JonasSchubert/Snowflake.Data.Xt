//-----------------------------------------------------------------------
// <copyright file="SnowflakeSchemaAttribute.cs" company="Jonas Schubert">
//     Copyright (c) Jonas Schubert. All rights reserved.
// </copyright>
// <author>Jonas Schubert</author>
//-----------------------------------------------------------------------

namespace Snowflake.Data.Xt
{
  /// <summary>
  /// The snowflake schema attribute.
  /// </summary>
  /// <remarks>
  /// Initializes a new instance of the <see cref="SnowflakeSchemaAttribute"/> class.
  /// </remarks>
  /// <param name="name">The name.</param>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public sealed class SnowflakeSchemaAttribute(string name) : Attribute
  {
    /// <summary>
    /// Gets the name.
    /// </summary>
    public string Name { get; internal set; } = name;
  }
}
