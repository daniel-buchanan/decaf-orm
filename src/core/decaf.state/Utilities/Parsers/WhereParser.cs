using System.Linq.Expressions;
using decaf.common;
using decaf.common.Exceptions;
using decaf.common.Utilities;
using decaf.common.Utilities.Reflection;
using decaf.state.Conditionals;

namespace decaf.state.Utilities.Parsers;

internal class WhereParser : BaseParser
{
    private readonly IParser joinParser;
    private readonly CallExpressionHelper callExpressionHelper;
    private readonly IParser valueParser;

    public WhereParser(
        IExpressionHelper expressionHelper,
        IReflectionHelper reflectionHelper,
        CallExpressionHelper callExpressionHelper,
        IParser joinParser,
        IParser valueParser)
        : base(expressionHelper, reflectionHelper)
    {
        this.callExpressionHelper = callExpressionHelper;
        this.joinParser = joinParser;
        this.valueParser = valueParser;
    }

    public override IWhere? Parse(Expression expression, IQueryContextExtended context)
    {
        ExpressionType nodeType;
        BinaryExpression? binaryExpr;
        IWhere? left, right;
        Expression body;

        var lambda = expression.CastAs<LambdaExpression>();
        if(lambda != null)
        {
            nodeType = lambda.Body.NodeType;
            body = lambda.Body;
        }
        else
        {
            nodeType = expression.NodeType;
            body = expression;
        }

        // check to see if we have a normal where
        if (nodeType != ExpressionType.OrElse && nodeType != ExpressionType.AndAlso)
        {
            return ParseInternal(expression, context);
        }

        // check for and
        if (nodeType == ExpressionType.AndAlso)
        {
            // get binary expression
            binaryExpr = body.ForceCastAs<BinaryExpression>();
            left = Parse(binaryExpr.Left, context);
            right = Parse(binaryExpr.Right, context);
            
            if (left is null || right is null)
                throw new TemplateParsingException($"Unable to parse where clause for {expression}");

            // return and
            return And.Where(left, right);
        }

        // check for or
        // get binary expression
        binaryExpr = body.ForceCastAs<BinaryExpression>();
        left = Parse(binaryExpr.Left, context);
        right = Parse(binaryExpr.Right, context);
        
        if (left is null || right is null)
            throw new TemplateParsingException($"Unable to parse where clause for {expression}");

        // return or
        return Or.Where(left, right);
    }

    private IWhere? ParseInternal(Expression expression, IQueryContextExtended context)
    {
        var earlyResult = callExpressionHelper.ParseExpression(expression, context);
        if (earlyResult != null) return earlyResult;

        BinaryExpression? binaryExpr = null;

        if (expression is not MemberExpression)
            binaryExpr = expression.CastAs<BinaryExpression>();

        if (binaryExpr is null) return valueParser.Parse(expression, context);

        IWhere? left, right;

        if (binaryExpr.NodeType == ExpressionType.AndAlso)
        {
            // get left and right
            left = ParseInternal(binaryExpr.Left, context);
            right = ParseInternal(binaryExpr.Right, context);

            if (left is null || right is null)
                throw new TemplateParsingException($"Unable to parse where clause for {expression}");

            // return and
            return And.Where(left, right);
        }
        if (binaryExpr.NodeType == ExpressionType.OrElse)
        {
            // get left and right
            left = ParseInternal(binaryExpr.Left, context);
            right = ParseInternal(binaryExpr.Right, context);

            if (left is null || right is null)
                throw new TemplateParsingException($"Unable to parse where clause for {expression}");
            
            // return or
            return Or.Where(left, right);
        }

        var operationTuple = UpdateNullableExpression(binaryExpr);
        var leftExpression = operationTuple.Item1;
        var rightExpression = operationTuple.Item2;

        var leftType = leftExpression.NodeType;
        var rightType = rightExpression.NodeType;

        var leftParam = ExpressionHelper.GetParameterName(leftExpression);
        var rightParam = ExpressionHelper.GetParameterName(rightExpression);
        var bothMemberAccess = leftType == ExpressionType.MemberAccess &&
                               rightType == ExpressionType.MemberAccess;
        var bothHaveParam = !string.IsNullOrWhiteSpace(leftParam) &&
                            !string.IsNullOrWhiteSpace(rightParam);

        if (bothMemberAccess && bothHaveParam)
            return joinParser.Parse(binaryExpr, context);

        // failing that parse a value clause
        return valueParser.Parse(binaryExpr, context);
    }
}