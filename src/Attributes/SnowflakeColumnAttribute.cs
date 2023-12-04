namespace Snowflake.Data.Xt;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class SnowflakeColumnAttribute : Attribute
{
  public SnowflakeColumnAttribute(string? name = default, string? table = default)
  {
    this.Name = name ?? string.Empty;
    this.Table = table;
  }

  public string Name { get; set; }

  public string? Table { get; }
}
