using System;
using System.Linq;
using System.Linq.Expressions;
using pdq.core.common;

namespace pdq.core.Implementation.Helpers
{
    class FieldExpressionHelper
    {
        private static SelectFunction[] HasParameters = new[] {SelectFunction.DatePart};
        private readonly ExpressionHelper expressionHelper;
        private readonly ReflectionHelper reflectionHelper;

        public FieldExpressionHelper(
            ExpressionHelper expressionHelper,
            ReflectionHelper reflectionHelper)
        {
            this.expressionHelper = expressionHelper;
            this.reflectionHelper = reflectionHelper;
        }

        public state.Column ParseFieldExpression<TSource>(Expression expression, string newName)
        {
            var field = state.Column.Create(
                    this.expressionHelper.GetName(expression),
                    state.Table.Create(this.expressionHelper.GetParameterName(expression)),
                    newName);

            var method = this.expressionHelper.GetMethodName(expression);
            if (method != null)
            {
                switch (method)
                {
                    case "ToLower":
                        field.Function = ValueFunction.ToLower;
                        break;
                    case "ToUpper":
                        field.Function = ValueFunction.ToUpper;
                        break;
                    case "DatePart":
                        field.Function = ValueFunction.DatePart;
                        break;
                    default:
                        field.Function = ValueFunction.None;
                        break;
                }

                if (string.IsNullOrWhiteSpace(newName)) newName = field.Field;
            }

            if(HasParameters.Contains(field.Function))
                ParseFunctionParameters(field, expression);

            if (!string.IsNullOrWhiteSpace(newName)) field.NewName = newName;

            return field;
        }

        private void ParseFunctionParameters(state.Column field, Expression expression)
        {
            if(expression.NodeType == ExpressionType.Lambda)
                ParseFunctionParameters(field, ((LambdaExpression)expression).Body);
            else if (expression.NodeType == ExpressionType.Convert)
                ParseFunctionParameters(field, ((UnaryExpression)expression).Operand);
            else if (expression.NodeType == ExpressionType.Call)
            {
                var call = (MethodCallExpression)expression;
                var countArguments = call.Arguments.Count;
                for (var i = 1; i < countArguments; i++)
                {
                    var arg = call.Arguments[i];
                    var value = this.expressionHelper.GetValue(arg);

                    // get underlying type of item
                    var underlyingType = this.reflectionHelper.GetUnderlyingType(value.GetType());

                    // create value parameter
                    var param = (IFunctionParameter)this.reflectionHelper.GetInstanceObject(typeof(ValueParam<>), underlyingType, new object[] { value });

                    // add to select
                    field.SqlFunctionParameters.Add(param);
                }
            }
        }
    }
}
