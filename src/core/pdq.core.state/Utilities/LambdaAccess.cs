using System;
using System.Linq.Expressions;

namespace pdq.state.Utilities
{
    class LambdaAccess
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
            return ((LambdaExpression)expression).Type;
        }

        public BinaryExpression GetBinaryExpression(Expression expression)
        {
            var lambda = (LambdaExpression)expression;
            return (BinaryExpression)lambda.Body;
        }
    }
}
