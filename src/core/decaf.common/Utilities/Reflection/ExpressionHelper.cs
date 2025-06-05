using System;
using System.Linq;
using System.Linq.Expressions;

namespace decaf.common.Utilities.Reflection
{
    public class ExpressionHelper(IReflectionHelper reflectionHelper) : IExpressionHelper
    {
        /// <inheritdoc/>
        public string GetMemberName(Expression expression)
        {
            // switch on node type
            switch (expression.NodeType)
            {
                case ExpressionType.Convert:
                    return ConvertAccess.GetName(expression, this);
                case ExpressionType.MemberAccess:
                    return MemberAccess.GetName(expression, reflectionHelper);
                case ExpressionType.Constant:
                    return MemberAccess.GetName(expression, reflectionHelper);
                case ExpressionType.Call:
                    {
                        var call = (MethodCallExpression)expression;
                        if (call.Object == null)
                        {
                            return GetMemberName(call.Arguments[0]);
                        }
                        return GetMemberName(call.Object);
                    }
                case ExpressionType.Lambda:
                    var body = ((LambdaExpression)expression).Body;
                    return GetMemberName(body);
            }

            // if not found, return null
            return null;
        }

        /// <inheritdoc/>
        public string GetMethodName(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Lambda) expression = ((LambdaExpression)expression).Body;
            if (expression.NodeType == ExpressionType.Convert) expression = ((UnaryExpression)expression).Operand;

            var methodExpression = expression as MethodCallExpression;
            if (methodExpression == null) return null;

            return methodExpression.Method.Name;
        }

        /// <inheritdoc/>
        public string GetParameterName(Expression expression)
        {
            // check the expression for null
            if (expression == null)
                throw new ArgumentNullException(nameof(expression), "The provided expression cannot be null.");

            // check for member expressions
            if (expression is MemberExpression { Expression: ParameterExpression parameterExpression })
            {
                return parameterExpression.Name;
            }

            if (expression is MethodCallExpression callExpression)
            {
                return GetParameterName(callExpression.Object ?? callExpression.Arguments[0]);
            }

            // if the expression is not a lambda, return nothing
            if (expression is not LambdaExpression lambdaExpr) return null;

            // get lambda and parameter
            var param = lambdaExpr.Parameters[0];

            // if no parameter, return null
            if (param == null)
                return null;

            // return parameter name
            return param.Name;
        }

        /// <inheritdoc/>
        public string GetTypeName<TObject>()
        {
            // use the reflection helper to get the table name
            // NOTE: this will use the [TableName] attribute if present
            return reflectionHelper.GetTableName(typeof(TObject));
        }

        /// <inheritdoc/>
        public EqualityOperator ConvertExpressionTypeToEqualityOperator(ExpressionType type)
        {
            // convert expression EqualityOperator into our EqualityOperator
            switch (type)
            {
                case ExpressionType.Equal:
                    return EqualityOperator.Equals;
                case ExpressionType.GreaterThan:
                    return EqualityOperator.GreaterThan;
                case ExpressionType.GreaterThanOrEqual:
                    return EqualityOperator.GreaterThanOrEqualTo;
                case ExpressionType.LessThan:
                    return EqualityOperator.LessThan;
                case ExpressionType.LessThanOrEqual:
                    return EqualityOperator.LessThanOrEqualTo;
                case ExpressionType.NotEqual:
                    return EqualityOperator.NotEquals;
            }

            // if nothing found, default to equals
            return EqualityOperator.Equals;
        }

        /// <inheritdoc/>
        public EqualityOperator ConvertExpressionTypeToEqualityOperator(Expression expression)
        {
            var binaryExpr = expression as BinaryExpression;
            if (binaryExpr == null) return EqualityOperator.Equals;
            return ConvertExpressionTypeToEqualityOperator(binaryExpr.NodeType);
        }

        /// <inheritdoc/>
        public object GetValue(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Convert)
            {
                try
                {
                    expression = ConvertAccess.GetExpression(expression);
                }
                catch
                {
                    return null;
                }
            }

            if (expression.NodeType == ExpressionType.MemberAccess)
            {
                return MemberAccess.GetValue(expression);
            }

            if (expression.NodeType == ExpressionType.Constant)
            {
                return ConstantAccess.GetValue(expression);
            }

            if (expression.NodeType == ExpressionType.Lambda)
            {
                var body = ((LambdaExpression)expression).Body;
                return GetValue(body);
            }

            if (expression.NodeType == ExpressionType.Call)
            {
                try
                {
                    return Expression.Lambda(expression).Compile().DynamicInvoke();
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }

        /// <inheritdoc/>
        public Type GetMemberType(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Convert)
            {
                return ConvertAccess.GetMemberType(expression, this);
            }
            else if (expression.NodeType == ExpressionType.MemberAccess)
            {
                return MemberAccess.GetMemberType(expression);
            }
            else if (expression.NodeType == ExpressionType.Constant)
            {
                return ConstantAccess.GetType(expression);
            }
            else if(expression.NodeType == ExpressionType.Parameter)
            {
                return ParameterAccess.GetType(expression);
            }
            else if (expression.NodeType == ExpressionType.Lambda)
            {
                var body = ((LambdaExpression)expression).Body;
                return GetMemberType(body);
            }

            return null;
        }

        /// <inheritdoc/>
        public Type GetParameterType(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Convert)
            {
                return ConvertAccess.GetParameterType(expression, this);
            }
            else if (expression.NodeType == ExpressionType.MemberAccess)
            {
                return MemberAccess.GetParameterType(expression);
            }
            else if (expression.NodeType == ExpressionType.Constant)
            {
                return ConstantAccess.GetType(expression);
            }
            else if (expression.NodeType == ExpressionType.Parameter)
            {
                return ParameterAccess.GetType(expression);
            }
            else if (expression.NodeType == ExpressionType.Lambda)
            {
                var body = ((LambdaExpression)expression).Body;
                return GetParameterType(body);
            }

            return null;
        }

        /// <inheritdoc/>
        public bool IsMethodCallOnProperty(Expression expression)
        {
            var methodCallExpression = expression as MethodCallExpression;
            var binaryExpression = expression as BinaryExpression;
            if (binaryExpression != null)
            {
                var call = binaryExpression.Left as MethodCallExpression ??
                           binaryExpression.Right as MethodCallExpression;
                var constant = binaryExpression.Left as ConstantExpression ??
                               binaryExpression.Right as ConstantExpression;

                if (call is null || constant is null) return false;
                methodCallExpression = call;
            }

            if (methodCallExpression == null) return false;
            var arguments = methodCallExpression.Arguments.ToArray();
            if (arguments.Length == 0) return false;

            if (methodCallExpression.Object is MemberExpression &&
                arguments.All(a => a.NodeType == ExpressionType.Constant) ||
                arguments.All(a => a.NodeType == ExpressionType.MemberAccess))
            {
                var memberExpression = methodCallExpression.Object as MemberExpression ??
                    arguments[0] as MemberExpression;
                var innerExpression = memberExpression?.Expression as ConstantExpression;
                return innerExpression is null;
            }

            var lastIndex = arguments.LastIndex();
            var lastArgument = arguments[lastIndex];
            return lastArgument.NodeType == ExpressionType.MemberAccess ||
                   lastArgument.NodeType == ExpressionType.Constant;
        }

        /// <inheritdoc/>
        public bool IsMethodCallOnConstantOrMemberAccess(Expression expression)
        {
            var methodCallExpression = expression as MethodCallExpression;
            if (methodCallExpression == null)
            {
                var binaryExpression = expression as BinaryExpression;
                methodCallExpression = binaryExpression?.Left as MethodCallExpression ??
                                       binaryExpression?.Right as MethodCallExpression;

                if (methodCallExpression is null) return false;
            }

            var arguments = methodCallExpression.Arguments.ToArray();
            if (arguments.Length == 0) return false;

            var firstArgument = methodCallExpression.Arguments[0];
            return firstArgument.NodeType == ExpressionType.MemberAccess ||
                   firstArgument.NodeType == ExpressionType.Constant;
        }
    }
}
