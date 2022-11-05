using System;
using pdq.state.Utilities;
using System.Linq.Expressions;
using System.Reflection;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("pdq.core-tests")]
namespace pdq.services
{
    internal static class ObjectExtensions
    {
        internal static readonly BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        private static string ParseExpression<T>(Expression<Func<T, object>> expression)
        {
            var reflectionHelper = new ReflectionHelper();
            var expressionHelper = new ExpressionHelper(reflectionHelper);
            return expressionHelper.GetName(expression);
        }

        internal static object GetProperty<T>(this T self, string property)
        {
            var type = typeof(T);
            var prop = type.GetProperty(property, Flags);
            if (prop == null) return null;
            return prop.GetValue(self);
        }

        internal static object GetProperty<T>(this T self, Expression<Func<T, object>> expression)
        {
            var propertyName = ParseExpression(expression);
            return self.GetProperty(propertyName);
        }

        internal static void SetProperty<T>(this T self, Expression<Func<T, object>> expression, object value)
        {
            var propertyName = ParseExpression(expression);
            self.SetProperty(propertyName, value);
        }

        internal static void SetProperty<T>(this T self, string property, object value)
        {
            var type = typeof(T);
            var prop = type.GetProperty(property, Flags);
            if (prop == null) return;
            prop.SetValue(self, value);
        }

        internal static void SetPropertyFrom<T>(this T self, string property, object source)
        {
            var newValue = source.GetProperty(property);
            self.SetProperty(property, newValue);
        }
    }
}

