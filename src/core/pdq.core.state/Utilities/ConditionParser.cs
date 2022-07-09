using System;
using System.Linq;
using System.Linq.Expressions;
using pdq.common;
using pdq.state.Conditionals;

namespace pdq.state.Utilities
{
    internal class ConditionParser
    {
        private readonly CallExpressionHelper callExpressionHelper;
        private readonly IExpressionHelper expressionHelper;
        private readonly IReflectionHelper reflectionHelper;
        private readonly IAliasManager aliasManager;
        private readonly IQueryContextInternal context;

        public ConditionParser(
            CallExpressionHelper callExpressionHelper,
            IExpressionHelper expressionHelper,
            IReflectionHelper reflectionHelper,
            IAliasManager aliasManager,
            IQueryContextInternal context)
        {
            this.callExpressionHelper = callExpressionHelper;
            this.expressionHelper = expressionHelper;
            this.reflectionHelper = reflectionHelper;
            this.aliasManager = aliasManager;
            this.context = context;
        }

        public state.IWhere ParseWhere(Expression expr, bool excludeAlias)
        {
            BinaryExpression binaryExpr = null;

            // check if we have a lambda
            if (expr.NodeType == ExpressionType.Lambda)
            {
                var lambda = (LambdaExpression)expr;

                // check for call expression on field
                if (lambda.Body.NodeType == ExpressionType.Call)
                    return callExpressionHelper.ParseCallExpressions(lambda.Body);

                binaryExpr = (BinaryExpression)lambda.Body;
            }
            // check if we have a method call
            else if (expr.NodeType == ExpressionType.Call)
            {
                // parse call expressions
                return callExpressionHelper.ParseCallExpressions(expr);
            }
            else
            {
                // otherwise make it a binary expression
                if (!(expr is MemberExpression)) binaryExpr = (BinaryExpression)expr;
            }

            if (binaryExpr == null) return ParseValue(expr, excludeAlias);

            IWhere left, right;

            if (binaryExpr.NodeType == ExpressionType.AndAlso)
            {
                // get left and right
                left = ParseWhere(binaryExpr.Left, excludeAlias);
                right = ParseWhere(binaryExpr.Right, excludeAlias);

                // return and
                return And.Where(left, right);
            }
            else if (binaryExpr.NodeType == ExpressionType.OrElse)
            {
                // get left and right
                left = ParseWhere(binaryExpr.Left, excludeAlias);
                right = ParseWhere(binaryExpr.Right, excludeAlias);

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

            if (bothMemberAccess && bothHaveParam) return ParseJoin(expr);

            // failing that parse a value clause
            return ParseValue(expr, excludeAlias);
        }

        public state.IWhere ParseJoin(Expression expr)
        {
            BinaryExpression binaryExpr = null;
            LambdaExpression lambdaExpr = null;

            if (expr.NodeType == ExpressionType.Lambda)
            {
                lambdaExpr = (LambdaExpression)expr;
                binaryExpr = (BinaryExpression)lambdaExpr.Body;
            }
            else
            {
                if (!(expr is MemberExpression)) binaryExpr = (BinaryExpression)expr;
            }

            if (binaryExpr != null && binaryExpr.NodeType == ExpressionType.AndAlso)
            {
                var left = ParseJoin(binaryExpr.Left);
                var right = ParseJoin(binaryExpr.Right);

                return And.Where(left, right);
            }
            else if (binaryExpr != null && binaryExpr.NodeType == ExpressionType.OrElse)
            {
                var left = ParseJoin(binaryExpr.Left);
                var right = ParseJoin(binaryExpr.Right);

                return Or.Where(left, right);
            }

            EqualityOperator op;

            if (expr is BinaryExpression)
            {
                BinaryExpression operation = (BinaryExpression)expr;

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
            else if (expr is LambdaExpression && lambdaExpr != null)
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

        private IQueryTarget GetQueryTarget(Expression expression)
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

        /// <summary>
        /// This is used for the case where we are doing a column.Match<T1,T2>((t1,t2) => t1.someNullable == t2.nonNull)
        /// We check for the null-able and then get the underlying expression to join against
        /// </summary>
        /// <param name="operation">The operation to check for null ables</param>
        /// <returns>A Tuple of Left,Right Expression</returns>
        private static Tuple<Expression, Expression> UpdateNullableExpression(
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
}
