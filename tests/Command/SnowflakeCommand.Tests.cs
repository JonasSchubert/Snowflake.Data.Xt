namespace Snowflake.Data.Xt.Tests;

public class SnowflakeCommandTests
{
  [Fact]
  public void Constructor_ShouldFill_SELECT_FROM_ForSimpleClass()
  {
    // Arrange
    var command = new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA");

    // Act
    var sql = command.Sql;

    // Assert
    sql.Should().Be("SELECT a.ID, a.PROP_1 FROM DATABASE.SCHEMA.SNOWFLAKE_CLASS1 AS a");
  }

  [Fact]
  public void Constructor_ShouldFill_SELECT_FROM_JOIN_ForClassWithJoin()
  {
    // Arrange
    var command = new SnowflakeCommand<SnowflakeClass2>("DATABASE", "SCHEMA");

    // Act
    var sql = command.Sql;

    // Assert
    sql.Should().Be("SELECT bar.ID, foo.PROP_1, bar.PROP_2 FROM DATABASE.SCHEMA.BAR AS bar LEFT JOIN DATABASE.SCHEMA.FOO AS foo ON bar.ID = foo.ID");
  }
}
