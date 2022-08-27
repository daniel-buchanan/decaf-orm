using System;
using pdq.state.Utilities;
using System.Linq.Expressions;
using System.Reflection;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("pdq.core-tests")]
namespace pdq.services
{
    internal static class ObjectExtensions
    {
        internal static void SetProperty<T>(this T self, Expression<Func<T, object>> expression, object value)
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

        internal static void SetProperty<T>(this T self, string property, object value)
        {
            var type = typeof(T);
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var prop = type.GetProperty(property, flags);
            if (prop == null) return;
            prop.SetValue(self, value);
        }
    }
}

