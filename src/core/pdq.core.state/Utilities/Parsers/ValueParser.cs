using System;
using System.Linq.Expressions;
using pdq.common;
using pdq.state.Conditionals;

namespace pdq.state.Utilities.Parsers
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

        public override state.IWhere Parse(Expression expression)
        {
            var earlyResult = ParseBinaryCallExpression(expression);
            if (earlyResult != null) return earlyResult;

            var valueResult = ParseMemberExpression(expression);
            if (valueResult == null) valueResult = ParseBinaryExpression(expression);

            var toCreate = typeof(Column<>);
            var col = state.Column.Create(valueResult.Field, state.QueryTargets.TableTarget.Create(valueResult.Table));

            //add the model type for the type def of the repository
            var args = new Type[] { valueResult.ValueType };
            var convertedValue = GetConvertedValue(valueResult.Value, valueResult.ValueType);
            var parameters = new[] { col, valueResult.Operator, convertedValue };

            //get the generic type definition for the model
            Type constructedField = toCreate.MakeGenericType(args);
            //create instance of the repository, typed for the model
            object instanceField = Activator.CreateInstance(constructedField, parameters);
            return (state.IWhere)instanceField;
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

        private IWhere ParseBinaryCallExpression(Expression expression)
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

            if (operation == null) return null;

            var left = operation.Left;
            var right = operation.Right;

            if (left.NodeType == ExpressionType.Call ||
                right.NodeType == ExpressionType.Call)
            {
                return callExpressionHelper.ParseBinaryCallExpressions(operation);
            }

            return null;
        }

        private ValueResult ParseMemberExpression(Expression expression)
        {
            if (!(expression is MemberExpression)) return null;

            var val = Parse(expression);
            var valType = this.expressionHelper.GetType(expression);
            var field = this.expressionHelper.GetName(expression);
            var table = this.expressionHelper.GetParameterName(expression);
            var op = EqualityOperator.Equals;
            return new ValueResult(field, table, val, valType, op);
        }

        private ValueResult ParseBinaryExpression(Expression expression)
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

            //start with left
            var val = Parse(operation.Left);
            var valType = this.expressionHelper.GetType(operation.Left);
            var field = this.expressionHelper.GetName(operation.Left);
            var table = this.expressionHelper.GetParameterName(operation.Left);

            //then right
            if (val == null) val = Parse(operation.Right);
            if (valType == null) valType = val == null ? this.expressionHelper.GetType(operation.Right) : val.GetType();
            if (String.IsNullOrEmpty(field)) field = this.expressionHelper.GetName(operation.Right);
            if (String.IsNullOrEmpty(table)) table = this.expressionHelper.GetParameterName(operation.Right);

            //get table if not already present
            if (String.IsNullOrEmpty(table)) table = this.expressionHelper.GetParameterName(expression);

            //get operation
            var op = this.expressionHelper.ConvertExpressionTypeToEqualityOperator(operation.NodeType);

            return new ValueResult(field, table, val, valType, op);
        }

        private sealed class ValueResult
        {
            public ValueResult() { }

            public ValueResult(string field, string table, object value, Type valueType, EqualityOperator op)
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
            public string Table { get; set; }
            public EqualityOperator Operator { get; set; }
        }
    }
}

