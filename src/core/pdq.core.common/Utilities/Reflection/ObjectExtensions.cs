using System;
using System.Linq.Expressions;
using System.Reflection;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("pdq.core-tests")]
namespace pdq.common.Utilities.Reflection
{
    public static class ObjectExtensions
    {
        internal static readonly BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public;

        private static string ParseExpression<T>(Expression<Func<T, object>> expression)
        {
            var reflectionHelper = new ReflectionHelper();
            var expressionHelper = new ExpressionHelper(reflectionHelper);
            return expressionHelper.GetMemberName(expression);
        }

        public static object GetPropertyValue(this object self, string property)
        {
            var type = self.GetType();
            var prop = type.GetProperty(property, Flags);
            if (prop == null) return null;
            return prop.GetValue(self);
        }

        public static object GetPropertyValue<T>(this T self, Expression<Func<T, object>> expression)
        {
            var propertyName = ParseExpression(expression);
            return self.GetPropertyValue(propertyName);
        }

        public static void SetProperty<T>(this T self, Expression<Func<T, object>> expression, object value)
        {
            var propertyName = ParseExpression(expression);
            self.SetPropertyValue(propertyName, value);
        }

        public static void SetPropertyValue<T>(this T self, string property, object value)
        {
            var type = typeof(T);
            var prop = type.GetProperty(property, Flags);
            if (prop == null) return;
            prop.SetValue(self, value);
        }

        public static void SetPropertyValueFrom<T>(this T self, string property, object source)
        {
            if (source == null) return;
            var newValue = source.GetPropertyValue(property);
            self.SetPropertyValue(property, newValue);
        }
    }
}

