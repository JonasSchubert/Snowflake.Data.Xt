namespace Snowflake.Data.Xt;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class SnowflakeColumnAttribute(string? name = default, string? table = default) : Attribute
{
  public string Name { get; set; } = name ?? string.Empty;

  public string? Table { get; } = table;
}
