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
    sql.Should().Be("SELECT a.ID, a.PROP_1 FROM DATABASE.SCHEMA.SnowflakeClass1 AS a");
  }

  [Fact]
  public void Constructor_ShouldFill_SELECT_FROM_JOIN_ForClassWithJoin()
  {
    // Arrange
    var command = new SnowflakeCommand<SnowflakeClass2>("DATABASE", "SCHEMA");

    // Act
    var sql = command.Sql;

    // Assert
    sql.Should().Be("SELECT bar.ID, foo.PROP_1, bar.Prop_2 FROM DATABASE.SCHEMA.BAR AS bar LEFT JOIN DATABASE.SCHEMA.FOO AS foo ON bar.ID = foo.ID");
  }

  [Fact]
  public void Constructor_ShouldFill_SELECT_FROM_JOIN_ForClassWithJoinOnItself()
  {
    // Arrange
    var command = new SnowflakeCommand<SnowflakeClass3>("DATABASE", "SCHEMA");

    // Act
    var sql = command.Sql;

    // Assert
    sql.Should().Be("SELECT bar1.ID, bar1.PROP_1, bar2.Prop_2 FROM DATABASE.SCHEMA.BAR AS bar1 LEFT JOIN DATABASE.SCHEMA.BAR AS bar2 ON bar1.ID = bar2.ID");
  }

  [Fact]
  public void Constructor_ShouldFill_SELECT_FROM_JOIN_ForClassWithJoinOnItselfAndDuplicatedColumns()
  {
    // Arrange
    var command = new SnowflakeCommand<SnowflakeClass4>("DATABASE", "SCHEMA");

    // Act
    var sql = command.Sql;

    // Assert
    sql.Should().Be("SELECT bar1.ID, bar1.PROP_1, bar1.Prop_2, bar2.Prop_2 AS Prop2Mapped FROM DATABASE.SCHEMA.BAR AS bar1 LEFT JOIN DATABASE.SCHEMA.BAR AS bar2 ON bar1.ID = bar2.ID");
  }
}
