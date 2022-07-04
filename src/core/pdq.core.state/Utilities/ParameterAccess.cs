using System;
using System.Linq.Expressions;

namespace pdq.state.Utilities
{
    class ParameterAccess
    {
        public object GetValue(Expression expression)
        {
            var objectMember = Expression.Convert(expression, expression.Type);
            var getterLambda = Expression.Lambda(objectMember);

            try
            {
                var getter = getterLambda.Compile();

                return getter.DynamicInvoke();
            }
            catch
            {
                return null;
            }
        }

        public Type GetType(Expression expression)
        {
            var parameterExpression = expression as ParameterExpression;
            if (parameterExpression == null) return null;
            return parameterExpression.Type;
        }

        public string GetName(Expression expression)
        {
            var parameterExpression = expression as ParameterExpression;
            if (parameterExpression == null) return null;
            return parameterExpression.Name;
        }
    }
}
