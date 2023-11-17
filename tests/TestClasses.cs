namespace Snowflake.Data.Xt.Tests;

[SnowflakeTable]
internal sealed class SnowflakeClass1
{
  [SnowflakeColumn("ID")]
  public int Id { get; set; }

  [SnowflakeColumn("PROP_1")]
  public string? Property1 { get; set; }
}

[SnowflakeTable(
  name: "BAR",
  alias: "bar")]
[SnowflakeJoin(
  table: "FOO",
  alias: "foo",
  type: SnowflakeJoinAttribute.Left,
  condition: "bar.ID = foo.ID")]
internal sealed class SnowflakeClass2
{
  [SnowflakeColumn("ID")]
  public int Id { get; set; }

  [SnowflakeColumn("PROP_1", "FOO")]
  public string? Property1 { get; set; }

  [SnowflakeColumn]
  public string? Prop_2 { get; set; }
}
