using System;
using System.Linq.Expressions;
using pdq.state.Conditionals;

namespace pdq.state.Utilities.Parsers
{
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

        public override state.IWhere Parse(Expression expression, IQueryContextInternal context)
        {
            // check to see if we have a normal where
            if (expression.NodeType != ExpressionType.OrElse && expression.NodeType != ExpressionType.AndAlso)
            {
                return Parse(expression, context, false);
            }

            // check for and
            if (expression.NodeType == ExpressionType.AndAlso)
            {
                // get binary expression
                var binaryExpr = (BinaryExpression)expression;

                // get left and right
                var left = Parse(binaryExpr.Left, context);
                var right = Parse(binaryExpr.Right, context);

                // return and
                return And.Where(left, right);
            }
            // check for or
            else if (expression.NodeType == ExpressionType.OrElse)
            {
                // get binary expression
                var binaryExpr = (BinaryExpression)expression;

                // get left and right
                var left = Parse(binaryExpr.Left, context);
                var right = Parse(binaryExpr.Right, context);

                // return or
                return Or.Where(left, right);
            }

            // otherwise return nothing
            return null;
        }

        private IWhere Parse(Expression expression, IQueryContextInternal context, bool excludeAlias)
        {
            var earlyResult = this.callExpressionHelper.ParseExpression(expression, context);
            if (earlyResult != null) return earlyResult;

            BinaryExpression binaryExpr = null;

            if (!(expression is MemberExpression))
                binaryExpr = expression as BinaryExpression;

            if (binaryExpr == null) return this.valueParser.Parse(expression, context);

            IWhere left, right;

            if (binaryExpr.NodeType == ExpressionType.AndAlso)
            {
                // get left and right
                left = Parse(binaryExpr.Left, context, excludeAlias);
                right = Parse(binaryExpr.Right, context, excludeAlias);

                // return and
                return And.Where(left, right);
            }
            else if (binaryExpr.NodeType == ExpressionType.OrElse)
            {
                // get left and right
                left = Parse(binaryExpr.Left, context, excludeAlias);
                right = Parse(binaryExpr.Right, context, excludeAlias);

                // return or
                return Or.Where(left, right);
            }

            var operationTuple = UpdateNullableExpression(binaryExpr);
            var leftExpression = operationTuple.Item1;
            var rightExpression = operationTuple.Item2;

            var leftType = leftExpression.NodeType;
            var rightType = rightExpression.NodeType;

            var leftParam = this.expressionHelper.GetParameterName(leftExpression);
            var rightParam = this.expressionHelper.GetParameterName(rightExpression);
            var bothMemberAccess = leftType == ExpressionType.MemberAccess &&
                                    rightType == ExpressionType.MemberAccess;
            var bothHaveParam = !string.IsNullOrWhiteSpace(leftParam) &&
                                !string.IsNullOrWhiteSpace(rightParam);

            if (bothMemberAccess && bothHaveParam)
                return this.joinParser.Parse(binaryExpr, context);

            // failing that parse a value clause
            return this.valueParser.Parse(binaryExpr, context);
        }
	}
}

