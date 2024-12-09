namespace Snowflake.Data.Xt.Tests
{
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

  [SnowflakeTable(
    name: "BAR",
    alias: "bar1")]
  [SnowflakeJoin(
    table: "BAR",
    alias: "bar2",
    type: SnowflakeJoinAttribute.Left,
    condition: "bar1.ID = bar2.ID")]
  internal sealed class SnowflakeClass3
  {
    [SnowflakeColumn("ID")]
    public int Id { get; set; }

    [SnowflakeColumn("PROP_1", "BAR")]
    public string? Property1 { get; set; }

    [SnowflakeColumn("Prop_2", "BAR", "bar2")]
    public string? Prop_2 { get; set; }
  }

  [SnowflakeTable(
    name: "BAR",
    alias: "bar1")]
  [SnowflakeJoin(
    table: "BAR",
    alias: "bar2",
    type: SnowflakeJoinAttribute.Left,
    condition: "bar1.ID = bar2.ID")]
  internal sealed class SnowflakeClass4
  {
    [SnowflakeColumn("ID")]
    public int Id { get; set; }

    [SnowflakeColumn("PROP_1", "BAR")]
    public string? Property1 { get; set; }

    [SnowflakeColumn("Prop_2", "BAR")]
    public string? Prop_2 { get; set; }

    [SnowflakeColumn("Prop_2", "BAR", "bar2", "Prop2Mapped")]
    public string? Prop_2_Mapped { get; set; }
  }

  [SnowflakeTable]
  internal sealed class SnowflakeClass5
  {
    [SnowflakeColumn("SnowflakeClass5")]
    public required string Name { get; set; }
  }
}
