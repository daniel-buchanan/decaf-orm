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

        public override state.IWhere Parse(Expression expr)
        {
            object val;
            Type valType;
            string field, table;
            EqualityOperator op;

            if (expr is MemberExpression)
            {
                val = Parse(expr);
                valType = this.expressionHelper.GetType(expr);
                field = this.expressionHelper.GetName(expr);
                table = this.expressionHelper.GetParameterName(expr);
                op = EqualityOperator.Equals;
            }
            else
            {
                BinaryExpression operation;
                if (expr.NodeType == ExpressionType.Lambda)
                {
                    var lambda = (LambdaExpression)expr;
                    operation = (BinaryExpression)lambda.Body;
                }
                else
                {
                    operation = (BinaryExpression)expr;
                }

                var left = operation.Left;
                var right = operation.Right;

                if (left.NodeType == ExpressionType.Call ||
                    right.NodeType == ExpressionType.Call)
                {
                    return callExpressionHelper.ParseBinaryCallExpressions(operation);
                }

                //start with left
                val = Parse(operation.Left);
                valType = this.expressionHelper.GetType(operation.Left);
                field = this.expressionHelper.GetName(operation.Left);
                table = this.expressionHelper.GetParameterName(operation.Left);

                //then right
                if (val == null) val = Parse(operation.Right);
                if (valType == null) valType = val == null ? this.expressionHelper.GetType(operation.Right) : val.GetType();
                if (String.IsNullOrEmpty(field)) field = this.expressionHelper.GetName(operation.Right);
                if (String.IsNullOrEmpty(table)) table = this.expressionHelper.GetParameterName(operation.Right);

                //get table if not already present
                if (String.IsNullOrEmpty(table)) table = this.expressionHelper.GetParameterName(expr);

                //get operation
                op = this.expressionHelper.ConvertExpressionTypeToEqualityOperator(operation.NodeType);
            }

            var toCreate = typeof(Column<>);
            var col = state.Column.Create(field, state.QueryTargets.TableTarget.Create(table));

            //add the model type for the type def of the repository
            Type[] args = { valType };

            object convertedValue;

            if (valType.IsEnum)
            {
                convertedValue = Enum.ToObject(valType, val);
            }
            else
            {
                if (val == null && valType == typeof(bool))
                {
                    convertedValue = true;
                }
                else if (val == null)
                {
                    convertedValue = null;
                }
                else
                {
                    try
                    {
                        convertedValue = Convert.ChangeType(val, valType);
                    }
                    catch
                    {
                        var underlyingType = reflectionHelper.GetUnderlyingType(valType);
                        convertedValue = Convert.ChangeType(val, underlyingType);
                    }
                }
            }

            var parameters = new[] { col, op, convertedValue };

            //get the generic type definition for the model
            Type constructedField = toCreate.MakeGenericType(args);
            //create instance of the repository, typed for the model
            object instanceField = Activator.CreateInstance(constructedField, parameters);
            return (state.IWhere)instanceField;
        }
    }
}

