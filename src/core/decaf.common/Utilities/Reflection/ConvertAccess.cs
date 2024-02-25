using System;
using System.Linq.Expressions;

namespace decaf.common.Utilities.Reflection
{
    static class ConvertAccess
    {
        private static Expression GetOperand(Expression expression)
        {
            var unaryExpression = (UnaryExpression)expression;
            return unaryExpression.Operand;
        }

        public static Expression GetExpression(Expression expression)
            => GetOperand(expression);

        public static object GetValue(Expression expression)
            => throw new NotImplementedException();

        public static Type GetMemberType(Expression expression, IExpressionHelper helper)
            => helper.GetMemberType(GetOperand(expression));

        public static Type GetParameterType(Expression expression, IExpressionHelper helper)
            => helper.GetParameterType(GetOperand(expression));

        public static string GetName(Expression expression, IExpressionHelper helper)
            => helper.GetMemberName(GetOperand(expression));
    }
}
