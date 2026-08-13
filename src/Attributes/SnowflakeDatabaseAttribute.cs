//-----------------------------------------------------------------------
// <copyright file="SnowflakeDatabaseAttribute.cs" company="Jonas Schubert">
//     Copyright (c) Jonas Schubert. All rights reserved.
// </copyright>
// <author>Jonas Schubert</author>
//-----------------------------------------------------------------------

namespace Snowflake.Data.Xt
{
  /// <summary>
  /// The snowflake database attribute.
  /// </summary>
  /// <remarks>
  /// Initializes a new instance of the <see cref="SnowflakeDatabaseAttribute"/> class.
  /// </remarks>
  /// <param name="name">The name.</param>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public sealed class SnowflakeDatabaseAttribute(string name) : Attribute
  {
    /// <summary>
    /// Gets the name.
    /// </summary>
    public string Name { get; internal set; } = name;
  }
}
