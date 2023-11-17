namespace Snowflake.Data.Xt;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class SnowflakeTableAttribute(string? name = default, string? alias = default) : Attribute
{
  public string? Alias { get; set; } = alias;

  public string Name { get; set; } = name ?? string.Empty;
}
