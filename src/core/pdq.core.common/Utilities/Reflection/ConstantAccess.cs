﻿using System;
using System.Linq.Expressions;

namespace pdq.common.Utilities.Reflection
{
    static class ConstantAccess
    {
        public static object GetValue(Expression expression)
        {
            return ((ConstantExpression)expression).Value;
        }

        public static Type GetType(Expression expression)
        {
            return ((ConstantExpression)expression).Type;
        }
    }
}
