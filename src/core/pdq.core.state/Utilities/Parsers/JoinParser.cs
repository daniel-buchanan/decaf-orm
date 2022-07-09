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
            BinaryExpression binaryExpr = null;
            LambdaExpression lambdaExpr = null;

            if (expression.NodeType == ExpressionType.Lambda)
            {
                lambdaExpr = (LambdaExpression)expression;
                binaryExpr = (BinaryExpression)lambdaExpr.Body;
            }
            else
            {
                if (!(expression is MemberExpression)) binaryExpr = (BinaryExpression)expression;
            }

            if (binaryExpr != null && binaryExpr.NodeType == ExpressionType.AndAlso)
            {
                var left = Parse(binaryExpr.Left);
                var right = Parse(binaryExpr.Right);

                return And.Where(left, right);
            }
            else if (binaryExpr != null && binaryExpr.NodeType == ExpressionType.OrElse)
            {
                var left = Parse(binaryExpr.Left);
                var right = Parse(binaryExpr.Right);

                return Or.Where(left, right);
            }

            EqualityOperator op;

            if (expression is BinaryExpression)
            {
                BinaryExpression operation = (BinaryExpression)expression;

                var operationTuple = UpdateNullableExpression(operation);
                var left = operationTuple.Item1;
                var right = operationTuple.Item2;

                //start with left
                var leftField = this.expressionHelper.GetName(left);
                var rightField = this.expressionHelper.GetName(right);

                //get operation
                op = this.expressionHelper.ConvertExpressionTypeToEqualityOperator(operation.NodeType);

                // create column
                return Conditionals.Column.Match(
                    Column.Create(leftField, GetQueryTarget(left)),
                    op,
                    Column.Create(rightField, GetQueryTarget(right)));
            }
            else if (expression is LambdaExpression && lambdaExpr != null)
            {
                BinaryExpression operation = binaryExpr;
                MemberExpression left;
                MemberExpression right;

                if (operation.Left is UnaryExpression)
                {
                    var unaryLeft = (UnaryExpression)operation.Left;
                    left = unaryLeft.Operand as MemberExpression;
                }
                else
                {
                    left = (MemberExpression)operation.Left;
                }

                if (operation.Right is UnaryExpression)
                {
                    var unaryRight = (UnaryExpression)operation.Right;
                    right = unaryRight.Operand as MemberExpression;
                }
                else
                {
                    right = (MemberExpression)operation.Right;
                }

                var leftParam = (ParameterExpression)left.Expression;
                var rightParam = (ParameterExpression)right.Expression;

                return Conditionals.Column.Match(
                    Column.Create(
                        reflectionHelper.GetFieldName(left.Member),
                        GetQueryTarget(leftParam)),
                    this.expressionHelper.ConvertExpressionTypeToEqualityOperator(operation.NodeType),
                    Column.Create(
                        reflectionHelper.GetFieldName(right.Member),
                        GetQueryTarget(rightParam)));
            }

            return null;
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
    }
}

