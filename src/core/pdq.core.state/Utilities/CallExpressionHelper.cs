using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using pdq.common;
using pdq.state.Conditionals;
using pdq.state.Conditionals.ValueFunctions;

namespace pdq.state.Utilities
{
    class CallExpressionHelper
    {
        private readonly IExpressionHelper expressionHelper;

        public CallExpressionHelper(
            IExpressionHelper expressionHelper)
        {
            this.expressionHelper = expressionHelper;
        }

        public state.IWhere ParseExpression(Expression expression, IQueryContextInternal context)
        {
            Expression expressionToParse;
            if (expression is LambdaExpression)
            {
                var lambda = expression as LambdaExpression;
                var unary = lambda.Body as UnaryExpression;
                if (unary?.NodeType == ExpressionType.Not)
                {
                    expressionToParse = Expression.Equal(unary.Operand, Expression.Constant(false));
                }
                else
                {
                    expressionToParse = lambda.Body;
                }
            }
            else
            {
                expressionToParse = expression;
            }

            if(IsMethodCallOnProperty(expressionToParse))
            {
                var binaryExpression = expressionToParse as BinaryExpression;
                if(binaryExpression == null)
                {
                    var constantExpression = Expression.Constant(true);
                    binaryExpression = Expression.Equal(expressionToParse, constantExpression);
                }
                return ParseBinaryExpression(binaryExpression, context);
            }

            if(IsMethodCallOnConstantOrMemberAccess(expressionToParse))
            {
                var binaryExpression = expressionToParse as BinaryExpression;
                if (binaryExpression == null)
                {
                    return ParseMethodAccessCall(expressionToParse, context);
                }

                if (binaryExpression.NodeType == ExpressionType.NotEqual)
                {
                    var call = binaryExpression.Left as MethodCallExpression ??
                               binaryExpression.Right as MethodCallExpression;
                    var result = ParseMethodAccessCall(call, context);
                    return Not.This(result);
                }
            }

            return ParseBinaryExpression(expressionToParse, context);
        }

        private bool IsMethodCallOnProperty(Expression expression)
        {
            var methodCallExpression = expression as MethodCallExpression;
            if (methodCallExpression == null) return false;

            if (methodCallExpression.Arguments.Count == 0) return false;

            if (methodCallExpression.Arguments.Count > 1) return false;

            if (methodCallExpression.Object is MemberExpression &&
                methodCallExpression.Arguments.All(a => a.NodeType == ExpressionType.Constant) ||
                methodCallExpression.Arguments.All(a => a.NodeType == ExpressionType.MemberAccess))
                return true;

            var lastArgument = methodCallExpression.Arguments.Last();
            return lastArgument.NodeType == ExpressionType.MemberAccess ||
                   lastArgument.NodeType == ExpressionType.Constant;
        }

        private bool IsMethodCallOnConstantOrMemberAccess(Expression expression)
        {
            var methodCallExpression = expression as MethodCallExpression;
            if (methodCallExpression == null) return false;

            if (methodCallExpression.Arguments.Count == 0) return false;

            var firstArgument = methodCallExpression.Arguments.First();
            return firstArgument.NodeType == ExpressionType.MemberAccess ||
                   firstArgument.NodeType == ExpressionType.Constant;
        }

        private state.IWhere ParseBinaryExpression(Expression expr, IQueryContextInternal context)
        {
            var binaryExpr = expr as BinaryExpression;
            if (binaryExpr == null)
            {
                var lambda = expr as LambdaExpression;
                binaryExpr = lambda.Body as BinaryExpression;
            }

            if (binaryExpr == null) return null;

            var left = binaryExpr.Left;
            var right = binaryExpr.Right;
            var nodeType = binaryExpr.NodeType;

            MethodCallExpression callExpr = null;
            Expression nonCallExpr = null;

            if (left.NodeType == ExpressionType.Call)
            {
                callExpr = (MethodCallExpression)left;
                nonCallExpr = right;
            }
            else if (right.NodeType == ExpressionType.Call)
            {
                callExpr = (MethodCallExpression)right;
                nonCallExpr = left;
            }

            if (callExpr == null) return null;

            state.IWhere result;

            switch(callExpr.Method.Name)
            {
                case SupportedMethods.DatePart:
                    result = ParseDatePartCall(nodeType, callExpr, nonCallExpr, context);
                    break;
                case SupportedMethods.ToLower:
                    result = ParseCaseCall(nodeType, callExpr, nonCallExpr, context);
                    break;
                case SupportedMethods.ToUpper:
                    result = ParseCaseCall(nodeType, callExpr, nonCallExpr, context);
                    break;
                case SupportedMethods.Substring:
                    result = ParseSubStringCall(nodeType, callExpr, nonCallExpr, context);
                    break;
                case SupportedMethods.Contains:
                    result = ParseContains(callExpr, nonCallExpr, context);
                    break;
                default:
                    result = null;
                    break;
            }

            if (result == null) return null;

            var op = this.expressionHelper.ConvertExpressionTypeToEqualityOperator(binaryExpr.NodeType);
            if (op == EqualityOperator.NotEquals)
                return Not.This(result);

            return result;
        }

        private state.IWhere ParseContains(
            MethodCallExpression callExpr,
            Expression nonCallExpr,
            IQueryContextInternal context)
        {
            var arg = callExpr.Arguments[0];
            var body = callExpr.Object;

            var memberAccessExp = body as MemberExpression;

            // get alias and field name
            var fieldName = this.expressionHelper.GetName(memberAccessExp);
            var value = this.expressionHelper.GetValue(arg) as string;

            if (nonCallExpr.NodeType != ExpressionType.Constant) return null;

            var constValue = (bool)this.expressionHelper.GetValue(nonCallExpr);
            var op = constValue ? EqualityOperator.Like : EqualityOperator.NotLike;

            var target = context.GetQueryTarget(memberAccessExp);
            var col = state.Column.Create(fieldName, target);

            if (value == null && op == EqualityOperator.NotLike)
                return Not.This(Conditionals.Column.Like<string>(col, null, StringContains.Create()));
            if (value == null)
                return Conditionals.Column.Like<string>(col, null, StringContains.Create());
            if (op == EqualityOperator.Like)
                return Conditionals.Column.Like(col, value, StringContains.Create());
            else return Not.This(Conditionals.Column.Like(col, value, StringContains.Create()));
        }

        private state.IWhere ParseDatePartCall(
            ExpressionType nodeType,
            MethodCallExpression callExpr,
            Expression nonCallExpr,
            IQueryContextInternal context)
        {
            var arguments = callExpr.Arguments;
            var objectExpression = arguments[0];
            var datePartExpression = arguments[1];
            var op = this.expressionHelper.ConvertExpressionTypeToEqualityOperator(nodeType);

            var dpField = this.expressionHelper.GetName(objectExpression);
            var dp = (common.DatePart)this.expressionHelper.GetValue(datePartExpression);

            var leftTarget = context.GetQueryTarget(objectExpression);
            var col = state.Column.Create(dpField, leftTarget);

            if (nonCallExpr.NodeType == ExpressionType.Constant)
            {
                var value = (int)this.expressionHelper.GetValue(nonCallExpr);
                return Conditionals.Column.Equals(col, op, value, state.Conditionals.ValueFunctions.DatePart.Create(dp));
            }

            if (nonCallExpr.NodeType == ExpressionType.MemberAccess)
            {
                var member = (MemberExpression) nonCallExpr;
                if (member.Expression.NodeType == ExpressionType.Constant)
                {
                    var value = (int)this.expressionHelper.GetValue(member);
                    return Conditionals.Column.Equals(col, op, value, state.Conditionals.ValueFunctions.DatePart.Create(dp));
                }

                var mField = this.expressionHelper.GetName(nonCallExpr);
                var target = context.GetQueryTarget(nonCallExpr);
                col = state.Column.Create(mField, target);

                return Conditionals.Column.Equals(col, op, 0, state.Conditionals.ValueFunctions.DatePart.Create(dp));
            }

            return null;
        }

        private ValueFunction ConvertMethodNameToValueFunction(string method)
        {
            if (method == "ToLower") return ValueFunction.ToLower;
            if (method == "ToUpper") return ValueFunction.ToUpper;
            if (method == "DatePart") return ValueFunction.DatePart;
            if (method == "Substring") return ValueFunction.Substring;

            return ValueFunction.None;
        }

        private state.IValueFunction ConvertMethodNameToValueFunctionImpl(ValueFunction function)
        {
            switch(function)
            {
                case ValueFunction.ToLower: return ToLower.Create();
                case ValueFunction.ToUpper: return ToUpper.Create();
                default: return null;
            }
        }

        private state.IWhere ParseCaseCall(
            ExpressionType nodeType,
            MethodCallExpression callExpr,
            Expression nonCallExpr,
            IQueryContextInternal context)
        {
            var body = callExpr.Object;
            var field = this.expressionHelper.GetName(body);
            var op = this.expressionHelper.ConvertExpressionTypeToEqualityOperator(nodeType);
            var leftTarget = context.GetQueryTarget(body);
            var col = state.Column.Create(field, leftTarget);

            var func = ConvertMethodNameToValueFunction(callExpr.Method.Name);

            if (func == ValueFunction.None) return null;
            var funcImplementation = ConvertMethodNameToValueFunctionImpl(func);


            if (nonCallExpr.NodeType == ExpressionType.Constant)
            {
                var value = this.expressionHelper.GetValue(nonCallExpr);
                var functionType = typeof(Conditionals.Column<>);
                var implementedType = functionType.MakeGenericType(value.GetType());
                return (state.IWhere)Activator.CreateInstance(implementedType, new object[] { col, op, value, funcImplementation });
            }

            if (nonCallExpr.NodeType == ExpressionType.MemberAccess)
            {
                var fieldB = this.expressionHelper.GetName(nonCallExpr);
                var target = context.GetQueryTarget(nonCallExpr);
                var right = state.Column.Create(fieldB, target);
                return Conditionals.Column.Equals(col, op, funcImplementation, right);
            }

            if (nonCallExpr.NodeType == ExpressionType.Call)
            {
                var rightCallExpr = (MethodCallExpression)nonCallExpr;
                var rightBody = rightCallExpr.Object;
                var fieldB = this.expressionHelper.GetName(rightBody);
                var methodB = ConvertMethodNameToValueFunction(rightCallExpr.Method.Name);
                if (methodB == ValueFunction.None) return null;

                var target = context.GetQueryTarget(rightBody);
                var right = state.Column.Create(fieldB, target);
                funcImplementation = ConvertMethodNameToValueFunctionImpl(methodB);

                return Conditionals.Column.Equals(col, op, funcImplementation, right);
            }

            return null;
        }

        private state.IWhere ParseSubStringCall(
            ExpressionType nodeType,
            MethodCallExpression callExpr,
            Expression nonCallExpr,
            IQueryContextInternal context)
        {
            var arguments = callExpr.Arguments;
            var startExpression = arguments[0];
            Expression lengthExpression = null;
            if (arguments.Count > 1) lengthExpression = arguments[1];

            var op = this.expressionHelper.ConvertExpressionTypeToEqualityOperator(nodeType);
            var field = this.expressionHelper.GetName(callExpr);
            var target = context.GetQueryTarget(callExpr);
            var col = state.Column.Create(field, target);

            var startValue = (int)this.expressionHelper.GetValue(startExpression);

            if (nonCallExpr.NodeType == ExpressionType.Constant)
            {
                var value = (string) this.expressionHelper.GetValue(nonCallExpr);

                if (lengthExpression != null)
                {
                    var lengthValue = (int) this.expressionHelper.GetValue(lengthExpression);
                    return Conditionals.Column.Equals(col, op, value, Substring.Create(startValue, lengthValue));
                }

                return Conditionals.Column.Equals(col, op, value, Substring.Create(startValue));
            }

            return null;
        }

        private state.IWhere ParseMethodAccessCall(Expression expression, IQueryContextInternal context)
        {
            var callExpression = expression as MethodCallExpression;
            if (callExpression == null) return null;

            state.IWhere result;
            switch(callExpression.Method.Name)
            {
                case SupportedMethods.Contains:
                    result = ParseVariableContainsAsValuesIn(callExpression, context);
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }

        private state.IWhere ParseVariableContainsAsValuesIn(MethodCallExpression call, IQueryContextInternal context)
        {
            var arg = call.Arguments[0];
            if (arg.NodeType != ExpressionType.MemberAccess) return null;

            MemberExpression memberAccessExp;
            Expression valueAccessExp;

            // check if this is a list or contains with variable
            if (call.Arguments.Count != 1)
            {
                // if the underlying value is an array
                memberAccessExp = call.Arguments[1] as MemberExpression;
                valueAccessExp = call.Arguments[0];
            }
            else
            {
                // check for passed in variable
                memberAccessExp = call.Arguments[0] as MemberExpression;
                valueAccessExp = call.Object;
            }

            if (memberAccessExp == null) return null;

            // get values
            var valueMember = valueAccessExp as MemberExpression;
            object values = this.expressionHelper.GetValue(valueMember);

            // get alias and field name
            var fieldName = this.expressionHelper.GetName(memberAccessExp);

            // setup arguments
            var typeArgs = new[] { memberAccessExp.Type };

            var inputGenericType = typeof(List<>);
            var inputType = inputGenericType.MakeGenericType(typeArgs);
            var input = Activator.CreateInstance(inputType, new object[] { values });
            var target = context.GetQueryTarget(memberAccessExp);
            var col = state.Column.Create(fieldName, target);

            var parameters = new object[] { col, input };
            var enumerableGenericType = typeof(IEnumerable<>);
            var enumerableType = enumerableGenericType.MakeGenericType(typeArgs);

            // get generic type
            var toCreate = typeof(InValues<>);
            var genericToCreate = toCreate.MakeGenericType(typeArgs);
            var parameterTypes = new[] { col.GetType(), enumerableType };
            var bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

            var ctor = genericToCreate.GetConstructor(bindingFlags, null, parameterTypes, null);

            if (ctor == null) return null;

            // return instance
            return (state.IWhere)ctor.Invoke(parameters);
        }

        public static class SupportedMethods
        {
            public const string DatePart = "DatePart";
            public const string ToLower = "ToLower";
            public const string ToUpper = "ToUpper";
            public const string Contains = "Contains";
            public const string Substring = "Substring";
        }
    }
}
