namespace Snowflake.Data.Xt.Tests
{
  public class SnowflakeCommandLimitTests
  {
    [Fact]
    public void Limit_ShouldAppend_LIMIT_OFFSET_ForSimpleClass()
    {
      // Arrange
      var command = new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
        .Limit(5);

      // Act
      var sql = command.Sql;

      // Assert
      sql.Should().Be("SELECT a.ID, a.PROP_1 FROM DATABASE.SCHEMA.SnowflakeClass1 AS a LIMIT 5 OFFSET 0");
    }

    [Fact]
    public void LimitAndOffset_ShouldAppend_LIMIT_OFFSET_ForSimpleClass()
    {
      // Arrange
      var command = new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
        .Limit(5, 3);

      // Act
      var sql = command.Sql;

      // Assert
      sql.Should().Be("SELECT a.ID, a.PROP_1 FROM DATABASE.SCHEMA.SnowflakeClass1 AS a LIMIT 5 OFFSET 3");
    }

    [Fact]
    public void Limit_ShouldAppend_LIMIT_OFFSET_ForClassWithJoin()
    {
      // Arrange
      var command = new SnowflakeCommand<SnowflakeClass2>("DATABASE", "SCHEMA")
        .Limit(5);

      // Act
      var sql = command.Sql;

      // Assert
      sql.Should().Be("SELECT bar.ID, foo.PROP_1, bar.Prop_2 FROM DATABASE.SCHEMA.BAR AS bar LEFT JOIN DATABASE.SCHEMA.FOO AS foo ON bar.ID = foo.ID LIMIT 5 OFFSET 0");
    }

    [Theory]
    [InlineData(0, -1)]
    [InlineData(-1, 0)]
    public void Top_ShouldThrowException_For0OrNegativeCountOrOffset_ForSimpleClass(int count, int offset)
    {
      // Arrange && Act
      var command = () => new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
        .Limit(count, offset);

      // Assert
      command.Should().Throw<ArgumentOutOfRangeException>().WithMessage(count is -1 ? "Count must be 0 or a positive integer! (Parameter 'count')" : "Offset must be 0 or a positive integer! (Parameter 'offset')");
    }

    [Fact]
    public void Top_ShouldThrowException_ForMultipleCalls_ForSimpleClass()
    {
      // Arrange && Act
      var command = () => new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
        .Limit(2)
        .Limit(2);

      // Assert
      command.Should().Throw<InvalidOperationException>().WithMessage("Command is already marked using limit!");
    }
  }
}
