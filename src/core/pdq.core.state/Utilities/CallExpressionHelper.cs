using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using pdq.common;
using pdq.state.Conditionals;
using pdq.state.Conditionals.ValueFunctions;

namespace pdq.state.Utilities
{
    class CallExpressionHelper
    {
        private ExpressionHelper expressionHelper;

        public CallExpressionHelper(ExpressionHelper expressionHelper)
        {
            this.expressionHelper = expressionHelper;
        }

        public state.IWhere ParseCallExpressions(Expression expr)
        {
            // get method call expression
            var call = (MethodCallExpression)expr;

            var arg = call.Arguments[0];
            var nodeType = arg.NodeType;

            // check for contains
            if (call.Method.Name == "Contains" && nodeType == ExpressionType.MemberAccess)
                return ParseContainsMemberAccessCall(call);
            if (call.Method.Name == "Contains" && nodeType == ExpressionType.Constant)
                return ParseContainsConstantCall(call);
            
            // otherwise return null
            return null;
        }

        public state.IWhere ParseBinaryCallExpressions(BinaryExpression expr)
        {
            var left = expr.Left;
            var right = expr.Right;
            var nodeType = expr.NodeType;

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

            if (callExpr.Method.Name == "DatePart") return ParseDatePartCall(nodeType, callExpr, nonCallExpr);
            if (callExpr.Method.Name == "ToLower") return ParseCaseCall(nodeType, callExpr, nonCallExpr);
            if (callExpr.Method.Name == "ToUpper") return ParseCaseCall(nodeType, callExpr, nonCallExpr);
            if (callExpr.Method.Name == "Substring") return ParseSubStringCall(nodeType, callExpr, nonCallExpr);
            if (callExpr.Method.Name == "Contains") return ParseStringContainsCall(callExpr, nonCallExpr);

            return null;
        }

        private state.IWhere ParseStringContainsCall(
            MethodCallExpression callExpr,
            Expression nonCallExpr)
        {
            var arg = callExpr.Arguments[0];
            var body = callExpr.Object;

            var memberAccessExp = body as MemberExpression;

            // get alias and field name
            var alias = this.expressionHelper.GetParameterName(memberAccessExp);
            var fieldName = this.expressionHelper.GetName(memberAccessExp);
            var value = this.expressionHelper.GetValue(arg) as string;

            if (nonCallExpr.NodeType != ExpressionType.Constant) return null;

            var constValue = (bool)this.expressionHelper.GetValue(nonCallExpr);
            var op = constValue ? EqualityOperator.Like : EqualityOperator.NotLike;

            var col = state.Column.Create(fieldName, state.QueryTargets.TableTarget.Create(alias));

            if (value == null && op == EqualityOperator.NotLike)
                return Conditionals.Column.NotLike<string>(col, null, StringContains.Create());
            if (value == null)
                return Conditionals.Column.Like<string>(col, null, StringContains.Create());
            if (op == EqualityOperator.Like)
                return Conditionals.Column.Like(col, value, StringContains.Create());
            else return Conditionals.Column.NotLike(col, value, StringContains.Create());
        }

        private state.IWhere ParseDatePartCall(
            ExpressionType nodeType,
            MethodCallExpression callExpr,
            Expression nonCallExpr)
        {
            var arguments = callExpr.Arguments;
            var objectExpression = arguments[0];
            var datePartExpression = arguments[1];
            var op = this.expressionHelper.ConvertExpressionTypeToEqualityOperator(nodeType);

            var dpAlias = this.expressionHelper.GetParameterName(objectExpression);
            var dpField = this.expressionHelper.GetName(objectExpression);
            var dp = (common.DatePart)this.expressionHelper.GetValue(datePartExpression);

            var col = state.Column.Create(dpField, state.QueryTargets.TableTarget.Create(dpAlias));

            if (nonCallExpr.NodeType == ExpressionType.Constant)
            {
                var value = (int)this.expressionHelper.GetValue(nonCallExpr);
                return Conditionals.Column.ValueMatch(col, op, value, state.Conditionals.ValueFunctions.DatePart.Create(dp));
            }

            if (nonCallExpr.NodeType == ExpressionType.MemberAccess)
            {
                var member = (MemberExpression) nonCallExpr;
                if (member.Expression.NodeType == ExpressionType.Constant)
                {
                    var value = (int)this.expressionHelper.GetValue(member);
                    return Conditionals.Column.ValueMatch(col, op, value, state.Conditionals.ValueFunctions.DatePart.Create(dp));
                }

                var mAlias = this.expressionHelper.GetParameterName(nonCallExpr);
                var mField = this.expressionHelper.GetName(nonCallExpr);
                col = state.Column.Create(mField, state.QueryTargets.TableTarget.Create(mAlias));

                return Conditionals.Column.ValueMatch(col, op, 0, state.Conditionals.ValueFunctions.DatePart.Create(dp));
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
            Expression nonCallExpr)
        {
            var body = callExpr.Object;
            var alias = this.expressionHelper.GetParameterName(body);
            var field = this.expressionHelper.GetName(body);
            var op = this.expressionHelper.ConvertExpressionTypeToEqualityOperator(nodeType);
            var col = state.Column.Create(field, state.QueryTargets.TableTarget.Create(alias));

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
                var aliasB = this.expressionHelper.GetParameterName(nonCallExpr);
                var fieldB = this.expressionHelper.GetName(nonCallExpr);
                var right = state.Column.Create(fieldB, state.QueryTargets.TableTarget.Create(aliasB));
                return Conditionals.Column.Match(col, op, funcImplementation, right);
            }

            if (nonCallExpr.NodeType == ExpressionType.Call)
            {
                var rightCallExpr = (MethodCallExpression)nonCallExpr;
                var rightBody = rightCallExpr.Object;
                var aliasB = this.expressionHelper.GetParameterName(rightBody);
                var fieldB = this.expressionHelper.GetName(rightBody);
                var methodB = ConvertMethodNameToValueFunction(rightCallExpr.Method.Name);
                if (methodB == ValueFunction.None) return null;

                var right = state.Column.Create(fieldB, state.QueryTargets.TableTarget.Create(aliasB));
                funcImplementation = ConvertMethodNameToValueFunctionImpl(methodB);

                return Conditionals.Column.Match(col, op, funcImplementation, right);
            }

            return null;
        }

        private state.IWhere ParseSubStringCall(
            ExpressionType nodeType,
            MethodCallExpression callExpr,
            Expression nonCallExpr)
        {
            var arguments = callExpr.Arguments;
            var startExpression = arguments[0];
            Expression lengthExpression = null;
            if (arguments.Count > 1) lengthExpression = arguments[1];

            var op = this.expressionHelper.ConvertExpressionTypeToEqualityOperator(nodeType);

            var alias = this.expressionHelper.GetParameterName(callExpr);
            var field = this.expressionHelper.GetName(callExpr);
            var col = state.Column.Create(field, state.QueryTargets.TableTarget.Create(alias));

            var startValue = (int)this.expressionHelper.GetValue(startExpression);

            if (nonCallExpr.NodeType == ExpressionType.Constant)
            {
                var value = (string) this.expressionHelper.GetValue(nonCallExpr);

                if (lengthExpression != null)
                {
                    var lengthValue = (int) this.expressionHelper.GetValue(lengthExpression);
                    return Conditionals.Column.ValueMatch(col, op, value, Substring.Create(startValue, lengthValue));
                }

                return Conditionals.Column.ValueMatch(col, op, value, Substring.Create(startValue));
            }

            return null;
        }

        private state.IWhere ParseContainsMemberAccessCall(MethodCallExpression call)
        {
            var arg = call.Arguments[0];
            if (arg.NodeType != ExpressionType.MemberAccess) return null;

            MemberExpression memberAccessExp;
            Expression valueAccessExp;

            // check if this is a list or contains with variable
            if (call.Arguments.Count == 1)
            {
                // check for passed in variable
                var firstArgument = call.Arguments[0];
                if (firstArgument.NodeType == ExpressionType.MemberAccess)
                {
                    // check if underlying value is a constant
                    var t = firstArgument as MemberExpression;
                    if (t.Expression.NodeType == ExpressionType.Constant)
                        return ParseContainsConstantCall(call);
                }

                // otherwise default to member access
                memberAccessExp = firstArgument as MemberExpression;
                valueAccessExp = call.Object;
            }
            else
            {
                // if the underlying value is an array
                memberAccessExp = call.Arguments[1] as MemberExpression;
                valueAccessExp = call.Arguments[0];
            }

            // get values
            var valueMember = valueAccessExp as MemberExpression;
            object values = this.expressionHelper.GetValue(valueMember);

            // get alias and field name
            var alias = this.expressionHelper.GetParameterName(memberAccessExp);
            var fieldName = this.expressionHelper.GetName(memberAccessExp);

            // setup arguments
            var typeArgs = new[] { memberAccessExp.Type };

            var inputGenericType = typeof(List<>);
            var inputType = inputGenericType.MakeGenericType(typeArgs);
            var input = Activator.CreateInstance(inputType, new object[] { values });
            var col = state.Column.Create(fieldName, state.QueryTargets.TableTarget.Create(alias));

            var parameters = new object[] { col, input };

            // get generic type
            var toCreate = typeof(Values<>);
            var genericToCreate = toCreate.MakeGenericType(typeArgs);

            var constructors = genericToCreate.GetConstructors();
            ConstructorInfo ctor = null;
            for (var i = 0; i < constructors.Length; i++)
            {
                var ctorParameters = constructors[i].GetParameters();

                if (ctorParameters.Length != parameters.Length)
                    continue;

                var useCtor = true;

                for (var p = 0; p < ctorParameters.Length; p++)
                {
                    useCtor &= ctorParameters[p].ParameterType == parameters[p].GetType();
                }

                if (!useCtor) continue;

                ctor = constructors[i];
            }

            // return instance
            return (state.IWhere)ctor.Invoke(parameters);
        }

        private state.IWhere ParseContainsConstantCall(MethodCallExpression call)
        {
            var arg = call.Arguments[0];
            var body = call.Object;

            var memberAccessExp = body as MemberExpression;

            // get alias and field name
            var alias = this.expressionHelper.GetParameterName(memberAccessExp);
            var fieldName = this.expressionHelper.GetName(memberAccessExp);
            var value = this.expressionHelper.GetValue(arg);
            var col = state.Column.Create(fieldName, state.QueryTargets.TableTarget.Create(alias));

            if (value == null)
                return Conditionals.Column.Like<string>(col, null, StringContains.Create());

            return Conditionals.Column.Like<string>(col, (string)value, StringContains.Create());
        }
    }
}
