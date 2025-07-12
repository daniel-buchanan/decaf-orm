using System.Linq.Expressions;
using decaf.common.ValueFunctions;

namespace decaf.common.Utilities.Reflection;

public interface IValueFunctionHelper
{
    IValueFunction? ParseFunction(Expression expression);
}

public class ValueFunctionHelper(IExpressionHelper expressionHelper) : IValueFunctionHelper
{
    public IValueFunction? ParseFunction(Expression expression)
    {
        if (expression is not MethodCallExpression callExpression) return null;

        return callExpression.Method.Name switch
        {
            SupportedMethods.ToLower => ToLower.Create(),
            SupportedMethods.ToUpper => ToUpper.Create(),
            SupportedMethods.DatePart => ParseDatePart(callExpression),
            SupportedMethods.Contains => ParseContains(callExpression),
            SupportedMethods.Substring => ParseSubString(callExpression),
            SupportedMethods.Trim => Trim.Create(),
            SupportedMethods.StartsWith => ParseStartsWith(callExpression),
            SupportedMethods.EndsWith => ParseEndsWith(callExpression),
            _ => null
        };
    }

    private IValueFunction ParseContains(MethodCallExpression expression)
    {
        var arg = expression.Arguments[0];
        var value = expressionHelper.GetValue(arg) as string;
        return StringContains.Create(value);
    }

    private IValueFunction ParseStartsWith(MethodCallExpression expression)
    {
        var arg = expression.Arguments[0];
        var value = expressionHelper.GetValue(arg) as string;
        return StringStartsWith.Create(value);
    }

    private IValueFunction ParseEndsWith(MethodCallExpression expression)
    {
        var arg = expression.Arguments[0];
        var value = expressionHelper.GetValue(arg) as string;
        return StringEndsWith.Create(value);
    }

    private IValueFunction ParseDatePart(MethodCallExpression expression)
    {
        var arguments = expression.Arguments;
        var datePartExpression = arguments[1];
        expressionHelper.TryGetValue<DatePart>(datePartExpression, out var dp);
        return ValueFunctions.DatePart.Create(dp);
    }

    private IValueFunction ParseSubString(MethodCallExpression expression)
    {
        var arguments = expression.Arguments;
        var startExpression = arguments[0];
        Expression? lengthExpression = null;
        if (arguments.Count > 1) lengthExpression = arguments[1];

        expressionHelper.TryGetValue<int>(startExpression, out var startValue);
        if (lengthExpression == null) return Substring.Create(startValue);
        expressionHelper.TryGetValue<int>(lengthExpression, out var lengthValue);
        
        return Substring.Create(startValue, lengthValue);

    }

    public static class SupportedMethods
    {
        public const string DatePart = "DatePart";
        public const string ToLower = "ToLower";
        public const string ToUpper = "ToUpper";
        public const string Contains = "Contains";
        public const string Substring = "Substring";
        public const string Trim = "Trim";
        public const string StartsWith = "StartsWith";
        public const string EndsWith = "EndsWith";
    }
}