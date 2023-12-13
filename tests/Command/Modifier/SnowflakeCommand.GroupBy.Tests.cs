namespace Snowflake.Data.Xt.Tests;

public class SnowflakeCommandGroupByTests
{
  [Fact]
  public void GroupByPredicate_ShouldFill_SELECT_FROM_ForSimpleClass_AndOneGroupBy()
  {
    // Arrange
    var command = new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
      .GroupBy(item => item.Property1);

    // Act
    var sql = command.Sql;

    // Assert
    sql.Should().Be("SELECT a.ID, a.PROP_1 FROM DATABASE.SCHEMA.SNOWFLAKE_CLASS1 AS a GROUP BY a.PROP_1");
  }

  [Fact]
  public void GroupByPredicate_ShouldFill_SELECT_FROM_JOIN_ForClassWithJoin_AndMultipleGroupBy()
  {
    // Arrange
    var command = new SnowflakeCommand<SnowflakeClass2>("DATABASE", "SCHEMA")
      .GroupBy(item => new { item.Property1, item.Prop_2 });

    // Act
    var sql = command.Sql;

    // Assert
    sql.Should().Be("SELECT bar.ID, foo.PROP_1, bar.PROP_2 FROM DATABASE.SCHEMA.BAR AS bar LEFT JOIN DATABASE.SCHEMA.FOO AS foo ON bar.ID = foo.ID GROUP BY foo.PROP_1, bar.PROP_2");
  }

  [Fact]
  public void GroupByString_ShouldFill_SELECT_FROM_ForSimpleClass()
  {
    // Arrange
    var command = new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
      .GroupBy("a.ID");

    // Act
    var sql = command.Sql;

    // Assert
    sql.Should().Be("SELECT a.ID, a.PROP_1 FROM DATABASE.SCHEMA.SNOWFLAKE_CLASS1 AS a GROUP BY a.ID");
  }

  [Fact]
  public void GroupByString_ShouldFill_SELECT_FROM_JOIN_ForClassWithJoin()
  {
    // Arrange
    var command = new SnowflakeCommand<SnowflakeClass2>("DATABASE", "SCHEMA")
      .GroupBy("GROUP BY bar.PROP_2");

    // Act
    var sql = command.Sql;

    // Assert
    sql.Should().Be("SELECT bar.ID, foo.PROP_1, bar.PROP_2 FROM DATABASE.SCHEMA.BAR AS bar LEFT JOIN DATABASE.SCHEMA.FOO AS foo ON bar.ID = foo.ID GROUP BY bar.PROP_2");
  }

  [Fact]
  public void GroupByString_ShouldThrowException_IfPropertyIsNotValid_ForSimpleClass()
  {
    // Arrange && Act
    var command = () => new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
      .GroupBy("a.ID;DROP *");

    // Assert
    command.Should().Throw<ArgumentException>().WithMessage("Group By may only contain valid properties (a.ID, a.PROP_1)! \"a.ID;DROP *\" is not allowed!");
  }

  [Fact]
  public void GroupByString_ShouldThrowException_IfPropertyIsNotValid_ForClassWithJoin()
  {
    // Arrange && Act
    var command = () => new SnowflakeCommand<SnowflakeClass2>("DATABASE", "SCHEMA")
      .GroupBy("GROUP BY bar.PROP_1");

    // Assert
    command.Should().Throw<ArgumentException>().WithMessage("Group By may only contain valid properties (bar.ID, foo.PROP_1, bar.PROP_2)! \"bar.PROP_1\" is not allowed!");
  }

  [Fact]
  public void GroupByString_ShouldThrowException_ForMultipleCalls_ForSimpleClass()
  {
    // Arrange && Act
    var command = () => new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
      .GroupBy("a.ID")
      .GroupBy("a.PROP_1");

    // Assert
    command.Should().Throw<InvalidOperationException>().WithMessage("Command already has a group by clause!");
  }

  [Fact]
  public void GroupByString_ShouldThrowException_ForMultipleCalls_ForClassWithJoin()
  {
    // Arrange && Act
    var command = () => new SnowflakeCommand<SnowflakeClass2>("DATABASE", "SCHEMA")
      .GroupBy("GROUP BY bar.ID")
      .GroupBy(item => item.Prop_2);

    // Assert
    command.Should().Throw<InvalidOperationException>().WithMessage("Command already has a group by clause!");
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("   ")]
  public void GroupByString_ShouldThrowException_ForEmptyArgument(string? groupBy)
  {
    // Arrange && Act
    var command = () => new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
# pragma warning disable CS8604
      .GroupBy(groupBy);
# pragma warning restore CS8604

    // Assert
    command.Should().Throw<ArgumentNullException>().WithMessage("Value for group by clause may not be empty! (Parameter 'groupBy')");
  }
}
