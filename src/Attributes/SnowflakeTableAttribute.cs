namespace Snowflake.Data.Xt;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class SnowflakeTableAttribute : Attribute
{
  public SnowflakeTableAttribute(string? name = default, string? alias = default)
  {
    this.Name = name ?? string.Empty;
    this.Alias = alias;
  }

  public string? Alias { get; set; }

  public string Name { get; set; }
}
