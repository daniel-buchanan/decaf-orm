using System;
using System.Linq.Expressions;
using System.Reflection;
using decaf.common;
using decaf.common.Utilities.Reflection;
using decaf.state.Conditionals;

namespace decaf.state.Utilities.Parsers
{
	internal class ValueParser : BaseParser
	{
        private readonly CallExpressionHelper callExpressionHelper;

        public ValueParser(
            IExpressionHelper expressionHelper,
            CallExpressionHelper callExpressionHelper,
            IReflectionHelper reflectionHelper)
            : base(expressionHelper, reflectionHelper)
        {
            this.callExpressionHelper = callExpressionHelper;
        }

        public override IWhere Parse(Expression expression, IQueryContextInternal context)
        {
            var earlyResult = callExpressionHelper.ParseExpression(expression, context);
            if (earlyResult != null) return earlyResult;

            var valueResult = ParseMemberExpression(expression, context);
            if (valueResult == null) valueResult = ParseBinaryExpression(expression, context);
            
            var col = state.Column.Create(valueResult.Field, valueResult.Table);

            var isNotEquals = valueResult.Operator == EqualityOperator.NotEquals;
            if (isNotEquals)
                valueResult.Operator = EqualityOperator.Equals;

            //add the model type for the type def of the repository
            var convertedValue = GetConvertedValue(valueResult.Value, valueResult.ValueType);
            var parameters = new[] { col, valueResult.Operator, convertedValue };
            var parameterTypes = new[] { col.GetType(), typeof(EqualityOperator), valueResult.ValueType };
            var bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

            //get the generic type definition for the model
            var genericType = typeof(Column<>);
            var genericTypeArguments = new Type[] { valueResult.ValueType };
            var constructedType = genericType.MakeGenericType(genericTypeArguments);
            var ctor = constructedType.GetConstructor(bindingFlags, null, parameterTypes, null);

            object instance;

            if(ctor != null) instance = ctor.Invoke(parameters);
            else instance = Activator.CreateInstance(constructedType, parameters);

            if (isNotEquals)
                return Not.This((IWhere)instance);

            return (IWhere)instance;
        }

        private object GetConvertedValue(object val, Type valType)
        {
            if (valType.IsEnum) return Enum.ToObject(valType, val);
            if (val == null && valType == typeof(bool)) return true;
            if (val == null) return null;

            object convertedValue;
            try
            {
                convertedValue = Convert.ChangeType(val, valType);
            }
            catch
            {
                var underlyingType = reflectionHelper.GetUnderlyingType(valType);
                convertedValue = Convert.ChangeType(val, underlyingType);
            }

            return convertedValue;
        }

        private ValueResult ParseMemberExpression(Expression expression, IQueryContextInternal context)
        {
            var memberExpression = expression as MemberExpression;
            if (memberExpression == null) return null;

            var val = Parse(expression, context);
            var valType = this.expressionHelper.GetMemberType(expression);
            var field = this.expressionHelper.GetMemberName(expression);
            var op = EqualityOperator.Equals;

            var target = context.GetQueryTarget(memberExpression);
            if (target == null)
            {
                var table = this.reflectionHelper.GetTableName(memberExpression.Member.DeclaringType);
                var alias = this.expressionHelper.GetParameterName(expression);
                target = QueryTargets.TableTarget.Create(table, alias);
                context.AddQueryTarget(target);
            }

            return new ValueResult(field, target, val, valType, op);
        }

        private ValueResult ParseBinaryExpression(Expression expression, IQueryContextInternal context)
        {
            BinaryExpression operation;
            if (expression.NodeType == ExpressionType.Lambda)
            {
                var lambda = expression as LambdaExpression;
                operation = lambda.Body as BinaryExpression;
            }
            else
            {
                operation = expression as BinaryExpression;
            }

            var op = this.expressionHelper.ConvertExpressionTypeToEqualityOperator(operation.NodeType);
            Expression valueExpression = null;
            MemberExpression memberExpression = null;

            if(operation.Left is MemberExpression)
            {
                memberExpression = operation.Left as MemberExpression;
                valueExpression = operation.Right;
            }
            else if(operation.Right is MemberExpression)
            {
                memberExpression = operation.Right as MemberExpression;
                valueExpression = operation.Left;
            }

            if (valueExpression == null || memberExpression == null)
                return null;

            var field = this.expressionHelper.GetMemberName(memberExpression);
            var value = this.expressionHelper.GetValue(valueExpression);
            var valueType = this.expressionHelper.GetMemberType(valueExpression);

            var target = context.GetQueryTarget(memberExpression);
            if(target == null)
            {
                var alias = this.expressionHelper.GetParameterName(memberExpression);
                var table = this.reflectionHelper.GetTableName(memberExpression.Member.DeclaringType);
                target = QueryTargets.TableTarget.Create(table, alias);
                context.AddQueryTarget(target);
            }

            return new ValueResult(field, target, value, valueType, op);
        }

        private sealed class ValueResult
        {
            public ValueResult() { }

            public ValueResult(string field, IQueryTarget table, object value, Type valueType, EqualityOperator op)
            {
                Field = field;
                Table = table;
                Value = value;
                ValueType = valueType;
                Operator = op;
            }

            public object Value { get; set; }
            public Type ValueType { get; set; }
            public string Field { get; set; }
            public IQueryTarget Table { get; set; }
            public EqualityOperator Operator { get; set; }
        }
    }
}

