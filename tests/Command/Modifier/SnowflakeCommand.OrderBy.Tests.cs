namespace Snowflake.Data.Xt.Tests
{
  public class SnowflakeCommandOrderByTests
  {
    [Fact]
    public void OrderByAscPredicate_ShouldFill_SELECT_FROM_ForSimpleClass_AndOneOrderBy()
    {
      // Arrange
      var command = new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
        .OrderByAsc(item => item.Property1);

      // Act
      var sql = command.Sql;

      // Assert
      sql.Should().Be("SELECT a.ID, a.PROP_1 FROM DATABASE.SCHEMA.SnowflakeClass1 AS a ORDER BY a.PROP_1 ASC");
    }

    [Fact]
    public void OrderByAscPredicate_ShouldFill_SELECT_FROM_JOIN_ForClassWithJoin_AndMultipleOrderBy()
    {
      // Arrange
      var command = new SnowflakeCommand<SnowflakeClass2>("DATABASE", "SCHEMA")
        .OrderByAsc(item => new { item.Property1, item.Prop_2 });

      // Act
      var sql = command.Sql;

      // Assert
      sql.Should().Be("SELECT bar.ID, foo.PROP_1, bar.Prop_2 FROM DATABASE.SCHEMA.BAR AS bar LEFT JOIN DATABASE.SCHEMA.FOO AS foo ON bar.ID = foo.ID ORDER BY foo.PROP_1, bar.Prop_2 ASC");
    }

    [Fact]
    public void OrderByDescPredicate_ShouldFill_SELECT_FROM_ForSimpleClass_AndOneOrderBy()
    {
      // Arrange
      var command = new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
        .OrderByDesc(item => item.Property1);

      // Act
      var sql = command.Sql;

      // Assert
      sql.Should().Be("SELECT a.ID, a.PROP_1 FROM DATABASE.SCHEMA.SnowflakeClass1 AS a ORDER BY a.PROP_1 DESC");
    }

    [Fact]
    public void OrderByDescPredicate_ShouldFill_SELECT_FROM_JOIN_ForClassWithJoin_AndMultipleOrderBy()
    {
      // Arrange
      var command = new SnowflakeCommand<SnowflakeClass2>("DATABASE", "SCHEMA")
        .OrderByDesc(item => new { item.Property1, item.Prop_2 });

      // Act
      var sql = command.Sql;

      // Assert
      sql.Should().Be("SELECT bar.ID, foo.PROP_1, bar.Prop_2 FROM DATABASE.SCHEMA.BAR AS bar LEFT JOIN DATABASE.SCHEMA.FOO AS foo ON bar.ID = foo.ID ORDER BY foo.PROP_1, bar.Prop_2 DESC");
    }

    [Fact]
    public void OrderByString_ShouldFill_SELECT_FROM_ForSimpleClass()
    {
      // Arrange
      var command = new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
        .OrderBy("a.ID ASC");

      // Act
      var sql = command.Sql;

      // Assert
      sql.Should().Be("SELECT a.ID, a.PROP_1 FROM DATABASE.SCHEMA.SnowflakeClass1 AS a ORDER BY a.ID ASC");
    }

    [Fact]
    public void OrderByString_ShouldFill_SELECT_FROM_JOIN_ForClassWithJoin()
    {
      // Arrange
      var command = new SnowflakeCommand<SnowflakeClass2>("DATABASE", "SCHEMA")
        .OrderBy("ORDER BY bar.Prop_2 ASC");

      // Act
      var sql = command.Sql;

      // Assert
      sql.Should().Be("SELECT bar.ID, foo.PROP_1, bar.Prop_2 FROM DATABASE.SCHEMA.BAR AS bar LEFT JOIN DATABASE.SCHEMA.FOO AS foo ON bar.ID = foo.ID ORDER BY bar.Prop_2 ASC");
    }

    [Fact]
    public void OrderByString_ShouldThrowException_IfPropertyIsNotValid_ForSimpleClass()
    {
      // Arrange && Act
      var command = () => new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
        .OrderBy("a.ID;DROP *");

      // Assert
      command.Should().Throw<ArgumentException>().WithMessage("Order By may only contain valid properties (a.ID, a.PROP_1)! \"a.ID;DROP *\" is not allowed!");
    }

    [Fact]
    public void OrderByString_ShouldThrowException_IfPropertyIsNotValid_ForClassWithJoin()
    {
      // Arrange && Act
      var command = () => new SnowflakeCommand<SnowflakeClass2>("DATABASE", "SCHEMA")
        .OrderBy("ORDER BY bar.PROP_1");

      // Assert
      command.Should().Throw<ArgumentException>().WithMessage("Order By may only contain valid properties (bar.ID, foo.PROP_1, bar.Prop_2)! \"bar.PROP_1\" is not allowed!");
    }

    [Fact]
    public void OrderByString_ShouldThrowException_ForMultipleCalls_ForSimpleClass()
    {
      // Arrange && Act
      var command = () => new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
        .OrderBy("a.ID")
        .OrderBy("a.PROP_1");

      // Assert
      command.Should().Throw<InvalidOperationException>().WithMessage("Command already has an order by clause!");
    }

    [Fact]
    public void OrderByString_ShouldThrowException_ForMultipleCalls_ForClassWithJoin()
    {
      // Arrange && Act
      var command = () => new SnowflakeCommand<SnowflakeClass2>("DATABASE", "SCHEMA")
        .OrderBy("ORDER BY bar.ID")
        .OrderByAsc(item => item.Prop_2);

      // Assert
      command.Should().Throw<InvalidOperationException>().WithMessage("Command already has an order by clause!");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void OrderByString_ShouldThrowException_ForEmptyArgument(string? orderBy)
    {
      // Arrange && Act
      var command = () => new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
# pragma warning disable CS8604
        .OrderBy(orderBy);
# pragma warning restore CS8604

      // Assert
      command.Should().Throw<ArgumentNullException>().WithMessage("Value for order by clause may not be empty! (Parameter 'orderBy')");
    }
  }
}
