using System;
using System.Linq.Expressions;
using decaf.common;
using decaf.common.Utilities;
using decaf.common.Utilities.Reflection;

namespace decaf.state.Utilities;

internal abstract class BaseParser(
    IExpressionHelper expressionHelper,
    IReflectionHelper reflectionHelper)
    : IParser
{
    protected readonly IExpressionHelper ExpressionHelper = expressionHelper;
    protected readonly IReflectionHelper ReflectionHelper = reflectionHelper;

    /// <inheritdoc/>
    public abstract IWhere? Parse(Expression expression, IQueryContextExtended context);

    /// <summary>
    /// This is used for the case where we are doing a column.Match
    /// We check for the null-able and then get the underlying expression to join against
    /// </summary>
    /// <param name="operation">The operation to check for null ables</param>
    /// <returns>A Tuple of Left,Right Expression</returns>
    protected static Tuple<Expression, Expression> UpdateNullableExpression(
        BinaryExpression operation)
    {
        var left = operation.Left;
        var right = operation.Right;
        if (left.NodeType == ExpressionType.Convert)
        {
            var temp = (UnaryExpression)left;
            left = temp.Operand;
        }

        if (right.NodeType == ExpressionType.Convert)
        {
            var temp = (UnaryExpression)right;
            right = temp.Operand;
        }

        return new Tuple<Expression, Expression>(left, right);
    }
}