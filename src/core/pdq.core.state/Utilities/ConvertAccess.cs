using System;
using System.Linq.Expressions;

namespace pdq.state.Utilities
{
    class ConvertAccess
    {
        private Expression GetOperand(Expression expression)
        {
            var unaryExpression = (UnaryExpression)expression;
            return unaryExpression.Operand;
        }

        public Expression GetExpression(Expression expression)
        {
            return GetOperand(expression);
        }

        public object GetValue(Expression expression)
        {
            throw new NotImplementedException();
        }

        public Type GetType(Expression expression, ExpressionHelper helper)
        {
            return helper.GetType(GetOperand(expression));
        }

        public string GetName(Expression expression, ExpressionHelper helper)
        {
            return helper.GetName(GetOperand(expression));
        }
    }
}
