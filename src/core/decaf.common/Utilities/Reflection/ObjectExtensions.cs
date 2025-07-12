using System;
using System.Linq.Expressions;
using System.Reflection;
using decaf.common.Exceptions;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("decaf.core.tests")]
namespace decaf.common.Utilities.Reflection;

public static class ObjectExtensions
{
    static readonly BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public;

    private static string? ParseExpression<T>(Expression<Func<T, object>> expression)
    {
        var reflectionHelper = new ReflectionHelper();
        var expressionHelper = new ExpressionHelper(reflectionHelper);
        return expressionHelper.GetMemberName(expression);
    }

    public static object? GetPropertyValue(this object self, string property)
    {
        var type = self.GetType();
        var prop = type.GetProperty(property, Flags);
        if (prop == null) return null;
        return prop.GetValue(self);
    }

    public static object? GetPropertyValue<T>(this T self, Expression<Func<T, object>> expression)
    {
        var propertyName = ParseExpression(expression);
        if(string.IsNullOrWhiteSpace(propertyName))
            throw new PropertyNotFoundException($"Property \"{expression}\" not found");
        return self!.GetPropertyValue(propertyName!);
    }

    public static void SetProperty<T>(this T self, Expression<Func<T, object>> expression, object value)
    {
        var propertyName = ParseExpression(expression);
        if(string.IsNullOrWhiteSpace(propertyName))
            throw new PropertyNotFoundException($"Property \"{expression}\" not found");
        self.SetPropertyValue(propertyName!, value);
    }

    public static void SetPropertyValue<T>(this T self, string property, object? value)
    {
        var type = typeof(T);
        var prop = type.GetProperty(property, Flags);
        if (prop == null) return;
        prop.SetValue(self, value);
    }

    public static void SetPropertyValueFrom<T>(this T self, string property, object? source)
    {
        if (source is null) return;
        var newValue = source.GetPropertyValue(property);
        self.SetPropertyValue(property, newValue);
    }
}