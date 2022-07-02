using System;
using System.Linq.Expressions;

namespace pdq.state.Utilities
{
    class ConstantAccess
    {
        public object GetValue(Expression expression)
        {
            return ((ConstantExpression)expression).Value;
        }

        public Type GetType(Expression expression)
        {
            return ((ConstantExpression)expression).Type;
        }
    }
}
