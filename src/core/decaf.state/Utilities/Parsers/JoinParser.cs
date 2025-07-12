using System.Linq.Expressions;
using decaf.common;
using decaf.common.Exceptions;
using decaf.common.Utilities;
using decaf.common.Utilities.Reflection;
using decaf.state.Conditionals;

namespace decaf.state.Utilities.Parsers;

internal class JoinParser(
    IExpressionHelper expressionHelper,
    IReflectionHelper reflectionHelper)
    : BaseParser(expressionHelper, reflectionHelper)
{
    public override IWhere? Parse(Expression expression, IQueryContextExtended context)
    {
        BinaryExpression? binaryExpression = null;
        LambdaExpression? lambdaExpression = null;

        if (expression is LambdaExpression lambda)
        {
            lambdaExpression = lambda;
            binaryExpression = lambda.Body as BinaryExpression;
        }
        else if (expression is not MemberExpression)
        {
            binaryExpression = expression as BinaryExpression;
        }

        if (ParseAndOr(binaryExpression, context, out var result) || 
            ParseLambdaClause(lambdaExpression, binaryExpression, context, out result)) 
            return result;

        return null;
    }

    private bool ParseAndOr(BinaryExpression? binaryExpression, IQueryContextExtended context, out IWhere? result)
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

            if (leftClause is null || rightClause is null)
                throw new TemplateParsingException("Unable to parse Join for \"AndAlso\"");
            
            result = And.Where(leftClause, rightClause);
            return true;
        }
        
        if (binaryExpression.NodeType == ExpressionType.OrElse)
        {
            var leftClause = Parse(binaryExpression.Left, context);
            var rightClause = Parse(binaryExpression.Right, context);

            if (leftClause is null || rightClause is null)
                throw new TemplateParsingException("Unable to parse Join for \"OrElse\"");
            
            result = Or.Where(leftClause, rightClause);
            return true;
        }

        return false;
    }

    private bool ParseLambdaClause(
        Expression? expression,
        BinaryExpression? binaryExpression,
        IQueryContextExtended context,
        out IWhere? result)
    {
        result = null;
        var lambdaExpression = expression as LambdaExpression;
        if (lambdaExpression == null) return false;

        var operation = binaryExpression;
        MemberExpression? leftExpression;
        MemberExpression? rightExpression;

        if (operation?.Left is UnaryExpression unaryLeft)
        {
            leftExpression = unaryLeft.Operand.CastAs<MemberExpression>();
        }
        else
        {
            leftExpression = operation?.Left?.CastAs<MemberExpression>();
        }

        switch (operation?.Right)
        {
            case UnaryExpression unaryRight:
                rightExpression = unaryRight.Operand.CastAs<MemberExpression>();
                break;
            case ConstantExpression:
                return false;
            default:
                rightExpression = operation?.Right.CastAs<MemberExpression>();
                break;
        }
        
        if(leftExpression is null || rightExpression is null)
            return false;

        var leftParam = (ParameterExpression)leftExpression.Expression;
        var rightParam = (ParameterExpression)rightExpression.Expression;

        var leftTarget = context.AddQueryTarget(leftParam);
        var leftField = ReflectionHelper.GetFieldName(leftExpression.Member);
        var op = ExpressionHelper.ConvertExpressionTypeToEqualityOperator(operation!.NodeType);
        var rightTarget = context.AddQueryTarget(rightParam);
        var rightField = ReflectionHelper.GetFieldName(rightExpression.Member);

        result = Conditionals.Column.Equals(
            Column.Create(leftField, leftTarget),
            op,
            Column.Create(rightField, rightTarget)
        );
        return true;
    }
}