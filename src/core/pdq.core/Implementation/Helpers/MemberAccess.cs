using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace pdq.core.Implementation.Helpers
{
    class MemberAccess
    {
        private readonly ReflectionHelper reflectionHelper;

        public MemberAccess(ReflectionHelper reflectionHelper)
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
