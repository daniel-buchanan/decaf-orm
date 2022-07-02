using System;
using System.Linq.Expressions;

namespace pdq.state.Utilities
{
    class MemberAccess
    {
        private readonly IReflectionHelper reflectionHelper;

        public MemberAccess(IReflectionHelper reflectionHelper)
        {
            this.reflectionHelper = reflectionHelper;
        }

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
            return ((MemberExpression)expression).Type;
        }

        public string GetName(Expression expression)
        {
            var memberExpr = (MemberExpression)expression;
            return this.reflectionHelper.GetFieldName(memberExpr.Member);
        }
    }
}
