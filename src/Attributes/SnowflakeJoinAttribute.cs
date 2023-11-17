namespace Snowflake.Data.Xt;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class SnowflakeJoinAttribute(string table, string alias, string type, string condition) : Attribute
{
  public const string Cross = "CROSS";

  public const string Full = "FULL";

  public const string FullOuter = "FULL OUTER";

  public const string Inner = "INNER";

  public const string Left = "LEFT";

  public const string LeftOuter = "LEFT OUTER";

  public const string Natural = "NATURAL";

  public const string Right = "RIGHT";

  public const string RightOuter = "RIGHT OUTER";

  public string Alias { get; set; } = alias;

  public string Condition { get; } = condition;

  public string Table { get; } = table;

  public string Type { get; } = type;
}
