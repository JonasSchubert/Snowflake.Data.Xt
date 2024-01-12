namespace Snowflake.Data.Xt.Tests;

public class SnowflakeCommandHavingTests
{
  [Fact]
  public void HavingString_ShouldFill_SELECT_FROM_ForSimpleClass()
  {
    // Arrange
    var command = new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
      .GroupBy(item => item.Id)
      .Having("count(*) > 1");

    // Act
    var sql = command.Sql;

    // Assert
    sql.Should().Be("SELECT a.ID, a.PROP_1 FROM DATABASE.SCHEMA.SnowflakeClass1 AS a GROUP BY a.ID HAVING count(*) > 1");
  }

  [Fact]
  public void HavingString_ShouldFill_SELECT_FROM_JOIN_ForClassWithJoin()
  {
    // Arrange
    var command = new SnowflakeCommand<SnowflakeClass2>("DATABASE", "SCHEMA")
      .GroupBy(item => item.Id)
      .Having("count(*) > 1");

    // Act
    var sql = command.Sql;

    // Assert
    sql.Should().Be("SELECT bar.ID, foo.PROP_1, bar.Prop_2 FROM DATABASE.SCHEMA.BAR AS bar LEFT JOIN DATABASE.SCHEMA.FOO AS foo ON bar.ID = foo.ID GROUP BY bar.ID HAVING count(*) > 1");
  }

  [Fact]
  public void HavingString_ShouldThrowException_ForMultipleCalls_ForSimpleClass()
  {
    // Arrange && Act
    var command = () => new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
      .GroupBy(item => item.Id)
      .Having("count(*) > 1")
      .Having("count(*) > 1");

    // Assert
    command.Should().Throw<InvalidOperationException>().WithMessage("Command already has a having clause!");
  }

  [Fact]
  public void HavingString_ShouldThrowException_ForMissingGroupBy_ForSimpleClass()
  {
    // Arrange && Act
    var command = () => new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
      .Having("count(*) > 1");

    // Assert
    command.Should().Throw<InvalidOperationException>().WithMessage("Group By is missing and has to be called before Having!");
  }

  [Fact]
  public void HavingString_ShouldThrowException_ForWrongOrderOfGroupBy_ForSimpleClass()
  {
    // Arrange && Act
    var command = () => new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
      .Having("count(*) > 1")
      .GroupBy(item => item.Id);

    // Assert
    command.Should().Throw<InvalidOperationException>().WithMessage("Group By is missing and has to be called before Having!");
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("   ")]
  public void HavingString_ShouldThrowException_ForEmptyArgument(string? having)
  {
    // Arrange && Act
    var command = () => new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
      .GroupBy(item => item.Id)
# pragma warning disable CS8604
      .Having(having);
# pragma warning restore CS8604

    // Assert
    command.Should().Throw<ArgumentNullException>().WithMessage("Value for having predicate may not be empty! (Parameter 'having')");
  }
}
