//-----------------------------------------------------------------------
// <copyright file="SnowflakeCommand.Where.cs" company="Jonas Schubert">
//     Copyright (c) Jonas Schubert. All rights reserved.
// </copyright>
// <author>Jonas Schubert</author>
//-----------------------------------------------------------------------

using System.Globalization;

namespace Snowflake.Data.Xt;

/// <summary>
/// The snowflake command.
/// </summary>
/// <typeparam name="T">The generic type. This is used to parse properties for the query.</typeparam>
public partial class SnowflakeCommand<T>
  where T : class
{
  /// <summary>
  /// Adds a where clause.
  /// https://docs.snowflake.com/en/sql-reference/constructs/where .
  /// </summary>
  /// <param name="predicate">The where predicate.</param>
  /// <returns>The snowflake command.</returns>
  /// <exception cref="InvalidOperationException">Command already has a where clause.</exception>
  /// <exception cref="NotSupportedException">If type is not supported.</exception>
  public SnowflakeCommand<T> Where(Expression<Func<T, bool>> predicate)
  {
    if (this.Sql.Contains("WHERE", StringComparison.Ordinal))
    {
      throw new InvalidOperationException("Command already has a where clause!");
    }

    var replacements = new Dictionary<string, string>(StringComparer.Ordinal);
    this.WalkExpression(ref replacements, predicate);

    var whereBody = predicate.Body.ToString();

    foreach (var parameter in predicate.Parameters)
    {
      whereBody = whereBody.Replace(parameter.Name + ".", string.Empty, StringComparison.Ordinal);
    }

    foreach (var replacement in replacements)
    {
      whereBody = whereBody.Replace(replacement.Key, "?", StringComparison.Ordinal);
    }

#pragma warning disable MA0026 // Fix TODO comment

    // TODO Add additional mappings, e.g. like https://github.com/phnx47/dapper-repositories/tree/main/src/SqlGenerator
#pragma warning restore MA0026 // Fix TODO comment
    foreach (var item in new (string, string)[]
      {
          ("AndAlso", "AND"),
          ("OrElse", "OR"),
          ("==", "="),
      })
    {
      whereBody = whereBody.Replace(item.Item1, item.Item2, StringComparison.Ordinal);
    }

    foreach (var property in this.Properties)
    {
      var propertyName = property.Value.Name;
      var propertyTableAlias = string.IsNullOrWhiteSpace(property.Value.Table)
        ? this.Table.Alias
        : this.Joins.Single(join => string.Equals(join.Table, property.Value.Table, StringComparison.Ordinal)).Alias;

      whereBody = whereBody.Replace($"{property.Key.Name}", $"{propertyTableAlias}.{propertyName}", StringComparison.Ordinal);
    }

    this.SqlBuilder.Append($" WHERE {whereBody.Trim()}");

    return this;
  }

  /// <summary>
  /// Adds a where clause.
  /// https://docs.snowflake.com/en/sql-reference/constructs/where .
  /// </summary>
  /// <param name="where">The where filter.</param>
  /// <returns>The snowflake command.</returns>
  /// <exception cref="InvalidOperationException">Command already has a where clause.</exception>
  /// <exception cref="ArgumentNullException">Value for where clause may not be empty.</exception>
  public SnowflakeCommand<T> Where(string where)
  {
    if (this.Sql.Contains("WHERE", StringComparison.Ordinal))
    {
      throw new InvalidOperationException("Command already has a where clause!");
    }

    if (string.IsNullOrWhiteSpace(where))
    {
      throw new ArgumentNullException(nameof(where), "Value for where clause may not be empty!");
    }

    this.SqlBuilder.Append($" {(where.Trim().StartsWith("WHERE", ignoreCase: true, CultureInfo.InvariantCulture) ? where.Trim() : $"WHERE {where.Trim()}")}");

    return this;
  }

  /// <summary>
  /// Walking the expression.
  /// </summary>
  /// <param name="replacements">The replacements.</param>
  /// <param name="expression">The expression.</param>
  /// <exception cref="NotSupportedException">If type is not supported.</exception>
  private void WalkExpression(ref Dictionary<string, string> replacements, Expression expression)
  {
    switch (expression.NodeType)
    {
      case ExpressionType.MemberAccess:
        var replacementExpression = expression.ToString();
        if (replacementExpression.Contains("value(", StringComparison.Ordinal))
        {
          if (!replacements.ContainsKey(replacementExpression))
          {
            var invocation = Expression.Lambda(expression).Compile().DynamicInvoke();

            var whereParameterType = invocation!.GetType();
            var whereParameterValue = invocation!.ToString() !;

            var dbType = this.typeMap[whereParameterType];
            var parameterIndex = string.Format(CultureInfo.InvariantCulture, "{0}", this.ParameterList.Count + 1);
            this.ParameterList.Add((parameterIndex, dbType, whereParameterValue));

            replacements.Add(replacementExpression, whereParameterValue);
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
        this.WalkExpression(ref replacements, binaryExpression!.Left);
        this.WalkExpression(ref replacements, binaryExpression!.Right);
        break;

      case ExpressionType.Call:
        var methodCallExpression = expression as MethodCallExpression;
        foreach (var argument in methodCallExpression!.Arguments)
        {
          this.WalkExpression(ref replacements, argument);
        }

        break;

      case ExpressionType.Lambda:
        var lambdaExpression = expression as LambdaExpression;
        this.WalkExpression(ref replacements, lambdaExpression!.Body);
        break;

      case ExpressionType.Constant:
        break;

      default:
        throw new NotSupportedException($"Unknown type \"{expression.NodeType}\".");
    }
  }
}
