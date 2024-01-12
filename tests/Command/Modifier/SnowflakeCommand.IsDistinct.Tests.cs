namespace Snowflake.Data.Xt.Tests;

public class SnowflakeCommandIsDistinctTests
{
  [Fact]
  public void IsDistinct_ShouldFill_SELECT_DISTINCT_FROM_ForSimpleClass()
  {
    // Arrange
    var command = new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
      .IsDistinct();

    // Act
    var sql = command.Sql;

    // Assert
    sql.Should().Be("SELECT DISTINCT a.ID, a.PROP_1 FROM DATABASE.SCHEMA.SnowflakeClass1 AS a");
  }

  [Fact]
  public void IsDistinct_ShouldFill_SELECT_FROM_JOIN_ForClassWithJoin()
  {
    // Arrange
    var command = new SnowflakeCommand<SnowflakeClass2>("DATABASE", "SCHEMA")
      .IsDistinct();

    // Act
    var sql = command.Sql;

    // Assert
    sql.Should().Be("SELECT DISTINCT bar.ID, foo.PROP_1, bar.Prop_2 FROM DATABASE.SCHEMA.BAR AS bar LEFT JOIN DATABASE.SCHEMA.FOO AS foo ON bar.ID = foo.ID");
  }

  [Fact]
  public void IsDistinct_ShouldThrowException_ForMultipleCalls_ForSimpleClass()
  {
    // Arrange && Act
    var command = () => new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
      .IsDistinct()
      .IsDistinct();

    // Assert
    command.Should().Throw<InvalidOperationException>().WithMessage("Command is already marked as distinct!");
  }
}
