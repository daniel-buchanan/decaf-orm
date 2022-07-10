using System;
using System.Linq;
using System.Linq.Expressions;
using pdq.common;
using pdq.state.Conditionals;

namespace pdq.state.Utilities.Parsers
{
	internal class JoinParser : BaseParser
	{
        private readonly IAliasManager aliasManager;
        private readonly IQueryContextInternal context;

        public JoinParser(
            IExpressionHelper expressionHelper,
            IReflectionHelper reflectionHelper,
            IAliasManager aliasManager,
            IQueryContextInternal context)
            : base(expressionHelper, reflectionHelper)
        {
            this.aliasManager = aliasManager;
            this.context = context;
        }

        public override IWhere Parse(Expression expression)
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
            if (ParseAndOr(binaryExpression, out result)) return result;
            if (ParseLambdaClause(lambdaExpression, binaryExpression, out result)) return result;

            return ParseSimpleBinary(expression);
        }

        protected IQueryTarget GetQueryTarget(Expression expression)
        {
            var alias = this.expressionHelper.GetParameterName(expression);
            var table = this.reflectionHelper.GetTableName(expression.Type);
            var managedAlias = this.aliasManager.FindByAssociation(table)?.FirstOrDefault()?.Name ?? alias;
            managedAlias = this.aliasManager.Add(managedAlias, table);
            var existing = this.context.QueryTargets.FirstOrDefault(t => t.Alias == managedAlias);

            if (existing != null) return existing;

            existing = QueryTargets.TableTarget.Create(managedAlias, managedAlias);
            this.context.AddQueryTarget(existing);
            return existing;
        }

        private bool ParseAndOr(BinaryExpression binaryExpression, out IWhere result)
        {
            result = null;
            if (binaryExpression == null) return false;
            if (binaryExpression.NodeType != ExpressionType.AndAlso &&
                binaryExpression.NodeType != ExpressionType.OrElse)
                return false;

            if (binaryExpression.NodeType == ExpressionType.AndAlso)
            {
                var leftClause = Parse(binaryExpression.Left);
                var rightClause = Parse(binaryExpression.Right);

                result = And.Where(leftClause, rightClause);
                return true;
            }
            else if (binaryExpression.NodeType == ExpressionType.OrElse)
            {
                var leftClause = Parse(binaryExpression.Left);
                var rightClause = Parse(binaryExpression.Right);

                result = Or.Where(leftClause, rightClause);
                return true;
            }

            return false;
        }

        private bool ParseLambdaClause(Expression expression, BinaryExpression binaryExpression, out IWhere result)
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
            else
            {
                rightExpression = operation.Right as MemberExpression;
            }

            var leftParam = (ParameterExpression)leftExpression.Expression;
            var rightParam = (ParameterExpression)rightExpression.Expression;

            result = Conditionals.Column.Match(
                Column.Create(
                    reflectionHelper.GetFieldName(leftExpression.Member),
                    GetQueryTarget(leftParam)),
                this.expressionHelper.ConvertExpressionTypeToEqualityOperator(operation.NodeType),
                Column.Create(
                    reflectionHelper.GetFieldName(rightExpression.Member),
                    GetQueryTarget(rightParam)));
            return true;
        }

        private IWhere ParseSimpleBinary(Expression expression)
        {
            if (!(expression is BinaryExpression)) return null;

            var operation = (BinaryExpression)expression;

            var operationTuple = UpdateNullableExpression(operation);
            var left = operationTuple.Item1;
            var right = operationTuple.Item2;

            //start with left
            var leftField = this.expressionHelper.GetName(left);
            var rightField = this.expressionHelper.GetName(right);

            //get operation
            var op = this.expressionHelper.ConvertExpressionTypeToEqualityOperator(operation.NodeType);

            // create column
            return Conditionals.Column.Match(
                Column.Create(leftField, GetQueryTarget(left)),
                op,
                Column.Create(rightField, GetQueryTarget(right)));
        }
    }
}

