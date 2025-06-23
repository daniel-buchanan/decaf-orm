using System.Linq.Expressions;
using decaf.common;
using decaf.common.Utilities.Reflection;
using decaf.state.Conditionals;

namespace decaf.state.Utilities.Parsers;

internal class JoinParser : BaseParser
{
    public JoinParser(
        IExpressionHelper expressionHelper,
        IReflectionHelper reflectionHelper)
        : base(expressionHelper, reflectionHelper)
    {
    }

    public override IWhere Parse(Expression expression, IQueryContextExtended context)
    {
        BinaryExpression binaryExpression = null;
        LambdaExpression lambdaExpression = null;

        if (expression.NodeType == ExpressionType.Lambda)
        {
            lambdaExpression = expression as LambdaExpression;
            binaryExpression = lambdaExpression.Body as BinaryExpression;
        }
        else
        {
            if (!(expression is MemberExpression))
                binaryExpression = expression as BinaryExpression;
        }

        IWhere result;
        if (ParseAndOr(binaryExpression, context, out result)) return result;
        if (ParseLambdaClause(lambdaExpression, binaryExpression, context, out result)) return result;

        return null;
    }

    private bool ParseAndOr(BinaryExpression binaryExpression, IQueryContextExtended context, out IWhere result)
    {
        result = null;
        if (binaryExpression == null) return false;
        if (binaryExpression.NodeType != ExpressionType.AndAlso &&
            binaryExpression.NodeType != ExpressionType.OrElse)
            return false;

        if (binaryExpression.NodeType == ExpressionType.AndAlso)
        {
            var leftClause = Parse(binaryExpression.Left, context);
            var rightClause = Parse(binaryExpression.Right, context);

            result = And.Where(leftClause, rightClause);
            return true;
        }
        else if (binaryExpression.NodeType == ExpressionType.OrElse)
        {
            var leftClause = Parse(binaryExpression.Left, context);
            var rightClause = Parse(binaryExpression.Right, context);

            result = Or.Where(leftClause, rightClause);
            return true;
        }

        return false;
    }

    private bool ParseLambdaClause(
        Expression expression,
        BinaryExpression binaryExpression,
        IQueryContextExtended context,
        out IWhere result)
    {
        result = null;
        if (expression == null) return false;
        var lambdaExpression = expression as LambdaExpression;
        if (lambdaExpression == null) return false;

        var operation = binaryExpression;
        MemberExpression leftExpression;
        MemberExpression rightExpression;

        if (operation.Left is UnaryExpression)
        {
            var unaryLeft = operation.Left as UnaryExpression;
            leftExpression = unaryLeft.Operand as MemberExpression;
        }
        else
        {
            leftExpression = operation.Left as MemberExpression;
        }

        if (operation.Right is UnaryExpression)
        {
            var unaryRight = operation.Right as UnaryExpression;
            rightExpression = unaryRight.Operand as MemberExpression;
        }
        else if (operation.Right is ConstantExpression)
        {
            return false;
        }
        else
        {
            rightExpression = operation.Right as MemberExpression;
        }

        var leftParam = (ParameterExpression)leftExpression.Expression;
        var rightParam = (ParameterExpression)rightExpression.Expression;

        var leftTarget = context.AddQueryTarget(leftParam);
        var leftField = reflectionHelper.GetFieldName(leftExpression.Member);
        var op = expressionHelper.ConvertExpressionTypeToEqualityOperator(operation.NodeType);
        var rightTarget = context.AddQueryTarget(rightParam);
        var rightField = reflectionHelper.GetFieldName(rightExpression.Member);

        result = Conditionals.Column.Equals(
            Column.Create(leftField, leftTarget),
            op,
            Column.Create(rightField, rightTarget)
        );
        return true;
    }
}