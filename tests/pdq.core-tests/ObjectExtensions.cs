using System;
using System.Linq.Expressions;
using System.Reflection;
using pdq.state.Utilities;

namespace pdq.core_tests
{
    public static class ObjectExtensions
    {
        public static void SetProperty<T>(this T self, Expression<Func<T, object>> expression, object value)
        {
            var type = typeof(T);
            var reflectionHelper = new ReflectionHelper();
            var expressionHelper = new ExpressionHelper(reflectionHelper);
            var propertyName = expressionHelper.GetName(expression);
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var prop = type.GetProperty(propertyName, flags);
            if (prop == null) return;
            prop.SetValue(self, value);
        }
    }
}

