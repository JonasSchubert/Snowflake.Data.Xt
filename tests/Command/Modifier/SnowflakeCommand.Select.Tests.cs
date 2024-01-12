namespace Snowflake.Data.Xt.Tests;

public class SnowflakeCommandSelectTests
{
  [Fact]
  public void SelectPredicate_ShouldFill_SELECT_FROM_ForSimpleClass_AndOneSelect()
  {
    // Arrange
    var command = new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
      .Select(item => item.Property1);

    // Act
    var sql = command.Sql;

    // Assert
    sql.Should().Be("SELECT a.PROP_1 FROM DATABASE.SCHEMA.SnowflakeClass1 AS a");
  }

  [Fact]
  public void SelectPredicate_ShouldFill_SELECT_FROM_JOIN_ForClassWithJoin_AndMultipleSelect()
  {
    // Arrange
    var command = new SnowflakeCommand<SnowflakeClass2>("DATABASE", "SCHEMA")
      .Select(item => new { item.Property1, item.Prop_2 });

    // Act
    var sql = command.Sql;

    // Assert
    sql.Should().Be("SELECT foo.PROP_1, bar.Prop_2 FROM DATABASE.SCHEMA.BAR AS bar LEFT JOIN DATABASE.SCHEMA.FOO AS foo ON bar.ID = foo.ID");
  }
}
