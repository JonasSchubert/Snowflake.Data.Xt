namespace Snowflake.Data.Xt.Tests;

public class SnowflakeCommandTopTests
{
  [Fact]
  public void Top_ShouldFill_SELECT_TOP_FROM_ForSimpleClass()
  {
    // Arrange
    var command = new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
      .Top(3);

    // Act
    var sql = command.Sql;

    // Assert
    sql.Should().Be("SELECT TOP 3 a.ID, a.PROP_1 FROM DATABASE.SCHEMA.SnowflakeClass1 AS a");
  }

  [Fact]
  public void Top_ShouldFill_SELECT_TOP_FROM_JOIN_ForClassWithJoin()
  {
    // Arrange
    var command = new SnowflakeCommand<SnowflakeClass2>("DATABASE", "SCHEMA")
      .Top(3);

    // Act
    var sql = command.Sql;

    // Assert
    sql.Should().Be("SELECT TOP 3 bar.ID, foo.PROP_1, bar.Prop_2 FROM DATABASE.SCHEMA.BAR AS bar LEFT JOIN DATABASE.SCHEMA.FOO AS foo ON bar.ID = foo.ID");
  }

  [Fact]
  public void Top_ShouldFill_SELECT_DISTINCT_TOP_FROM_ForSimpleClass()
  {
    // Arrange
    var command = new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
      .IsDistinct()
      .Top(3);

    // Act
    var sql = command.Sql;

    // Assert
    sql.Should().Be("SELECT DISTINCT TOP 3 a.ID, a.PROP_1 FROM DATABASE.SCHEMA.SnowflakeClass1 AS a");
  }

  [Fact]
  public void Top_ShouldFill_SELECT_DISTINCT_TOP_FROM_JOIN_ForClassWithJoin()
  {
    // Arrange
    var command = new SnowflakeCommand<SnowflakeClass2>("DATABASE", "SCHEMA")
      .IsDistinct()
      .Top(3);

    // Act
    var sql = command.Sql;

    // Assert
    sql.Should().Be("SELECT DISTINCT TOP 3 bar.ID, foo.PROP_1, bar.Prop_2 FROM DATABASE.SCHEMA.BAR AS bar LEFT JOIN DATABASE.SCHEMA.FOO AS foo ON bar.ID = foo.ID");
  }

  [Theory]
  [InlineData(0)]
  [InlineData(-1)]
  public void Top_ShouldThrowException_For0OrNegativeAmount_ForSimpleClass(int amount)
  {
    // Arrange && Act
    var command = () => new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
      .Top(amount);

    // Assert
    command.Should().Throw<ArgumentOutOfRangeException>().WithMessage("Amount must be a positive integer! (Parameter 'amount')");
  }

  [Fact]
  public void Top_ShouldThrowException_ForMultipleCalls_ForSimpleClass()
  {
    // Arrange && Act
    var command = () => new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
      .Top(2)
      .Top(2);

    // Assert
    command.Should().Throw<InvalidOperationException>().WithMessage("Command is already marked using top amount!");
  }
}
