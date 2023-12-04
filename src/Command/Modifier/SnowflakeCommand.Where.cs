namespace Snowflake.Data.Xt;

public partial class SnowflakeCommand<T>
  where T : class
{
  /// <summary>
  /// Adds a where clause.
  /// https://docs.snowflake.com/en/sql-reference/constructs/where
  /// </summary>
  /// <param name="predicate">The where predicate.</param>
  /// <returns>The snowflake command.</returns>
  /// <exception cref="InvalidOperationException">Command already has a where clause!</exception>
  /// <exception cref="NotSupportedException">If type is not supported.</exception>
  public SnowflakeCommand<T> Where(Expression<Func<T, bool>> predicate)
  {
    if (this.Sql.Contains("WHERE"))
    {
      throw new InvalidOperationException("Command already has a where clause!");
    }

		var replacements = new Dictionary<string, string>();
    WalkExpression(ref replacements, predicate);

    var whereBody = predicate.Body.ToString();

    foreach (var parameter in predicate.Parameters)
    {
        whereBody = whereBody.Replace(parameter.Name + ".", string.Empty);
    }

    foreach (var replacement in replacements)
    {
        whereBody = whereBody.Replace(replacement.Key, replacement.Value);   
    }

    foreach (var item in new (string, string)[]
      {
          ("AndAlso", "AND"),
          ("OrElse", "OR"),
          ("==", "="),
        // TODO Add additional mappings, e.g. like https://github.com/phnx47/dapper-repositories/tree/main/src/SqlGenerator
      })
    {
      whereBody = whereBody.Replace(item.Item1, item.Item2);
    }

    foreach (var property in this.Properties)
    {
      var propertyName = property.Value.Name;
      var propertyTableAlias = string.IsNullOrWhiteSpace(property.Value.Table)
        ? this.Table.Alias
        : this.Joins.Single(join => join.Table == property.Value.Table).Alias;

      whereBody = whereBody.Replace($"{property.Key.Name}", $"{propertyTableAlias}.{propertyName}");
    }

    this.SqlBuilder.Append($" WHERE {whereBody.Trim()}");

    return this;
  }

  /// <summary>
  /// Adds a where clause.
  /// https://docs.snowflake.com/en/sql-reference/constructs/where
  /// </summary>
  /// <param name="where">The where filter.</param>
  /// <returns>The snowflake command.</returns>
  /// <exception cref="InvalidOperationException">Command already has a where clause!</exception>
  /// <exception cref="ArgumentNullException">Value for where clause may not be empty!</exception>
  public SnowflakeCommand<T> Where(string where)
  {
    if (this.Sql.Contains("WHERE"))
    {
      throw new InvalidOperationException("Command already has a where clause!");
    }

    if (string.IsNullOrWhiteSpace(where))
    {
      throw new ArgumentNullException(nameof(where), "Value for where clause may not be empty!");
    }

    this.SqlBuilder.Append($" {(where.Trim().StartsWith("WHERE", ignoreCase: true, null) ? where.Trim() : $"WHERE {where.Trim()}")}");

    return this;
  }

  /// <summary>
  /// Walking the expression.
  /// </summary>
  /// <param name="replacements">The replacements.</param>
  /// <param name="expression">The expression.</param>
  /// <exception cref="NotSupportedException">If type is not supported.</exception>
  private static void WalkExpression(ref Dictionary<string, string> replacements, Expression expression)
  {
    switch (expression.NodeType)
    {
      case ExpressionType.MemberAccess:
        var replacementExpression = expression.ToString();
        if (replacementExpression.Contains("value("))
        {
          if (!replacements.ContainsKey(replacementExpression))
          {
            var invocation = Expression.Lambda(expression).Compile().DynamicInvoke();
            var replacementType = invocation!.GetType().ToString();
            var replacementValue = replacementType == "System.String" ? $"\"{invocation}\"" : invocation.ToString();

            replacements.Add(replacementExpression, replacementValue!.ToString());
          }
        }
        break;

      case ExpressionType.GreaterThan:
      case ExpressionType.GreaterThanOrEqual:
      case ExpressionType.LessThan:
      case ExpressionType.LessThanOrEqual:
      case ExpressionType.OrElse:
      case ExpressionType.AndAlso:
      case ExpressionType.Equal:
        var binaryExpression = expression as BinaryExpression;
        WalkExpression(ref replacements, binaryExpression!.Left);
        WalkExpression(ref replacements, binaryExpression!.Right);
        break;

      case ExpressionType.Call:
        var methodCallExpression = expression as MethodCallExpression;
        foreach (var argument in methodCallExpression!.Arguments)
        {
          WalkExpression(ref replacements, argument);
        }
        break;

      case ExpressionType.Lambda:
        var lambdaExpression = expression as LambdaExpression;
        WalkExpression(ref replacements, lambdaExpression!.Body);
        break;

      case ExpressionType.Constant:
        break;

      default:
        throw new NotSupportedException($"Unknown type \"{expression.NodeType}\".");
    }
  }
}
