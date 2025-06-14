namespace Snowflake.Data.Xt.Tests
{
  public class SnowflakeCommandWhereTests
  {
    [Fact]
    public void WherePredicate_ShouldFill_SELECT_FROM_ForSimpleClass_AndOneWhere()
    {
      // Arrange && Act
      var command = new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
        .Where(item => item.Property1 == "test");

      // Assert
      command.Sql.Should().Be("SELECT a.ID, a.PROP_1 FROM DATABASE.SCHEMA.SnowflakeClass1 AS a WHERE (a.PROP_1 = 'test')");
      command.ParameterList.Count.Should().Be(0);
    }

    [Fact]
    public void WherePredicate_ShouldFill_SELECT_FROM_ForSimpleClass_AndOneWhereWithParameter()
    {
      // Arrange && ACt
      var testProperty = "test";
      var command = new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
        .Where(item => item.Property1 == testProperty);

      // Assert
      command.Sql.Should().Be("SELECT a.ID, a.PROP_1 FROM DATABASE.SCHEMA.SnowflakeClass1 AS a WHERE (a.PROP_1 = ?)");
      command.ParameterList.Count.Should().Be(1);
      command.ParameterList[0].Item1.Should().Be("1");
      command.ParameterList[0].Item2.Should().Be(System.Data.DbType.String);
      command.ParameterList[0].Item3.Should().Be("test");
    }

    [Fact]
    public void WherePredicate_ShouldFill_SELECT_FROM_JOIN_ForClassWithJoin_AndMultipleWhere()
    {
      // Arrange && Act
      var command = new SnowflakeCommand<SnowflakeClass2>("DATABASE", "SCHEMA")
        .Where(item => item.Id == 2 && item.Property1 == "test");

      // Assert
      command.Sql.Should().Be("SELECT bar.ID, foo.PROP_1, bar.Prop_2 FROM DATABASE.SCHEMA.BAR AS bar LEFT JOIN DATABASE.SCHEMA.FOO AS foo ON bar.ID = foo.ID WHERE ((bar.ID = 2) AND (foo.PROP_1 = 'test'))");
      command.ParameterList.Count.Should().Be(0);
    }

    [Fact]
    public void WherePredicate_ShouldFill_SELECT_FROM_ForSimpleClass_AndMultipleWhereWithParameter()
    {
      // Arrange && Act
      var testProperty = "test";
      var command = new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
        .Where(item => item.Property1 == testProperty || (item.Id > 4 && item.Id < 10));

      // Assert
      command.Sql.Should().Be("SELECT a.ID, a.PROP_1 FROM DATABASE.SCHEMA.SnowflakeClass1 AS a WHERE ((a.PROP_1 = ?) OR ((a.ID > 4) AND (a.ID < 10)))");
      command.ParameterList.Count.Should().Be(1);
      command.ParameterList[0].Item1.Should().Be("1");
      command.ParameterList[0].Item2.Should().Be(System.Data.DbType.String);
      command.ParameterList[0].Item3.Should().Be("test");
    }

    [Fact]
    public void WherePredicate_ShouldFill_SELECT_FROM_ForClassWithPropertyNameAsClassName_AndMultipleWhereWithParameter()
    {
      // Arrange && Act
      var testProperty = "test";
      var command = new SnowflakeCommand<SnowflakeClass5>("DATABASE", "SCHEMA")
        .Where(item => item.Name == testProperty || item.Name == string.Empty || item.Name == null);

      // Assert
      command.Sql.Should().Be("SELECT a.SnowflakeClass5 FROM DATABASE.SCHEMA.SnowflakeClass5 AS a WHERE (((a.SnowflakeClass5 = ?) OR (a.SnowflakeClass5 = '')) OR (a.SnowflakeClass5 = null))");
      command.ParameterList.Count.Should().Be(1);
      command.ParameterList[0].Item1.Should().Be("1");
      command.ParameterList[0].Item2.Should().Be(System.Data.DbType.String);
      command.ParameterList[0].Item3.Should().Be("test");
    }

    [Fact]
    public void WhereString_ShouldFill_SELECT_FROM_ForSimpleClass()
    {
      // Arrange && Act
      var command = new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
        .Where("a.ID != 2");

      // Assert
      command.Sql.Should().Be("SELECT a.ID, a.PROP_1 FROM DATABASE.SCHEMA.SnowflakeClass1 AS a WHERE a.ID != 2");
      command.ParameterList.Count.Should().Be(0);
    }

    [Fact]
    public void WhereString_ShouldFill_SELECT_FROM_JOIN_ForClassWithJoin()
    {
      // Arrange && Act
      var command = new SnowflakeCommand<SnowflakeClass2>("DATABASE", "SCHEMA")
        .Where("WHERE bar.Prop_2 == 'test'");

      // Assert
      command.Sql.Should().Be("SELECT bar.ID, foo.PROP_1, bar.Prop_2 FROM DATABASE.SCHEMA.BAR AS bar LEFT JOIN DATABASE.SCHEMA.FOO AS foo ON bar.ID = foo.ID WHERE bar.Prop_2 == 'test'");
      command.ParameterList.Count.Should().Be(0);
    }

    [Fact]
    public void WhereString_ShouldThrowException_ForMultipleCalls_ForSimpleClass()
    {
      // Arrange && Act
      var command = () => new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
        .Where("a.ID != 2")
        .Where("a.PROP_1 == 'test'");

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
    public void WhereString_ShouldThrowException_ForEmptyArgument(string? where)
    {
      // Arrange && Act
#pragma warning disable CS8604 // Possible null reference argument.
      var command = () => new SnowflakeCommand<SnowflakeClass1>("DATABASE", "SCHEMA")
        .Where(where);
#pragma warning restore CS8604 // Possible null reference argument.

      // Assert
      command.Should().Throw<ArgumentNullException>().WithMessage("Value for where clause may not be empty! (Parameter 'where')");
    }
  }
}
