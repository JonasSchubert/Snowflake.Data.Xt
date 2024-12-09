//-----------------------------------------------------------------------
// <copyright file="SnowflakeJoinAttribute.cs" company="Jonas Schubert">
//     Copyright (c) Jonas Schubert. All rights reserved.
// </copyright>
// <author>Jonas Schubert</author>
//-----------------------------------------------------------------------

namespace Snowflake.Data.Xt
{
  /// <summary>
  /// The snowflake join attribute.
  /// </summary>
  /// <remarks>
  /// Initializes a new instance of the <see cref="SnowflakeJoinAttribute"/> class.
  /// </remarks>
  /// <param name="table">The table.</param>
  /// <param name="alias">The alias.</param>
  /// <param name="type">The type.</param>
  /// <param name="condition">The condition.</param>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
  public sealed class SnowflakeJoinAttribute(string table, string alias, string type, string condition) : Attribute
  {
    /// <summary>
    /// Defines a join of type CROSS.
    /// </summary>
    public const string Cross = "CROSS";

    /// <summary>
    /// Defines a join of type FULL.
    /// </summary>
    public const string Full = "FULL";

    /// <summary>
    /// Defines a join of type FULL OUTER.
    /// </summary>
    public const string FullOuter = "FULL OUTER";

    /// <summary>
    /// Defines a join of type INNER.
    /// </summary>
    public const string Inner = "INNER";

    /// <summary>
    /// Defines a join of type LEFT.
    /// </summary>
    public const string Left = "LEFT";

    /// <summary>
    /// Defines a join of type LEFT OUTER.
    /// </summary>
    public const string LeftOuter = "LEFT OUTER";

    /// <summary>
    /// Defines a join of type NATURAL.
    /// </summary>
    public const string Natural = "NATURAL";

    /// <summary>
    /// Defines a join of type RIGHT.
    /// </summary>
    public const string Right = "RIGHT";

    /// <summary>
    /// Defines a join of type RIGHT OUTER.
    /// </summary>
    public const string RightOuter = "RIGHT OUTER";

    /// <summary>
    /// Gets the alias.
    /// </summary>
    public string Alias { get; internal set; } = alias;

    /// <summary>
    /// Gets the condition.
    /// </summary>
    public string Condition { get; } = condition;

    /// <summary>
    /// Gets the table.
    /// </summary>
    public string Table { get; } = table;

    /// <summary>
    /// Gets the type.
    /// </summary>
    public string Type { get; } = type;
  }
}
