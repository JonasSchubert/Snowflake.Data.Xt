namespace Snowflake.Data.Xt.Tests;

public class SnowflakeCommandWhereTests
{
  [Fact]
  public void WherePredicate_ShouldFill_SELECT_FROM_ForSimpleClass_AndOneWhere()
  {
    // Arrange
    var command = new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
      .Where(item => item.Property1 == "test");

    // Act
    var sql = command.Sql;

    // Assert
    sql.Should().Be("SELECT a.ID, a.PROP_1 FROM DATABASE.SCHEMA.SNOWFLAKE_CLASS1 AS a WHERE (a.PROP_1 = \"test\")");
  }

  [Fact]
  public void WherePredicate_ShouldFill_SELECT_FROM_JOIN_ForClassWithJoin_AndMultipleWhere()
  {
    // Arrange
    var command = new SnowflakeCommand<SnowflakeClass2>("DATABASE", "SCHEMA")
      .Where(item => item.Id == 2 && item.Property1 == "test");

    // Act
    var sql = command.Sql;

    // Assert
    sql.Should().Be("SELECT bar.ID, foo.PROP_1, bar.PROP_2 FROM DATABASE.SCHEMA.BAR AS bar LEFT JOIN DATABASE.SCHEMA.FOO AS foo ON bar.ID = foo.ID WHERE ((bar.ID = 2) AND (foo.PROP_1 = \"test\"))");
  }
  
  [Fact]
  public void WhereString_ShouldFill_SELECT_FROM_ForSimpleClass()
  {
    // Arrange
    var command = new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
      .Where("a.ID != 2");

    // Act
    var sql = command.Sql;

    // Assert
    sql.Should().Be("SELECT a.ID, a.PROP_1 FROM DATABASE.SCHEMA.SNOWFLAKE_CLASS1 AS a WHERE a.ID != 2");
  }

  [Fact]
  public void WhereString_ShouldFill_SELECT_FROM_JOIN_ForClassWithJoin()
  {
    // Arrange
    var command = new SnowflakeCommand<SnowflakeClass2>("DATABASE", "SCHEMA")
      .Where("WHERE bar.PROP_2 == \"test\"");

    // Act
    var sql = command.Sql;

    // Assert
    sql.Should().Be("SELECT bar.ID, foo.PROP_1, bar.PROP_2 FROM DATABASE.SCHEMA.BAR AS bar LEFT JOIN DATABASE.SCHEMA.FOO AS foo ON bar.ID = foo.ID WHERE bar.PROP_2 == \"test\"");
  }
 
  [Fact]
  public void WhereString_ShouldThrowException_ForMultipleCalls_ForSimpleClass()
  {
    // Arrange && Act
    var command = () => new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
      .Where("a.ID != 2")
      .Where("a.PROP_1 == \"test\"");

    // Assert
    command.Should().Throw<InvalidOperationException>().WithMessage("Command already has a where clause!");
  }

  [Fact]
  public void WhereString_ShouldThrowException_ForMultipleCalls_ForClassWithJoin()
  {
    // Arrange && Act
    var command = () => new SnowflakeCommand<SnowflakeClass2>("DATABASE", "SCHEMA")
      .Where("WHERE bar.ID > 2")
      .Where(item => item.Prop_2 == "test");

    // Assert
    command.Should().Throw<InvalidOperationException>().WithMessage("Command already has a where clause!");
  }
  
  [Theory]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("   ")]
  public void WhereString_ShouldThrowException_ForEmptyArgument(string where)
  {
    // Arrange && Act
    var command = () => new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
      .Where(where);

    // Assert
    command.Should().Throw<ArgumentNullException>().WithMessage("Value for where clause may not be empty! (Parameter 'where')");
  }
}
