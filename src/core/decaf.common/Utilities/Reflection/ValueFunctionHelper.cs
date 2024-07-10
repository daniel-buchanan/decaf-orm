using System.Linq.Expressions;
using decaf.common.ValueFunctions;

namespace decaf.common.Utilities.Reflection
{
    public interface IValueFunctionHelper
    {
        IValueFunction ParseFunction(Expression expression);
    }

    public class ValueFunctionHelper : IValueFunctionHelper
    {
        private readonly IExpressionHelper expressionHelper;

        public ValueFunctionHelper(
            IExpressionHelper expressionHelper)
        {
            this.expressionHelper = expressionHelper;
        }

        public IValueFunction ParseFunction(Expression expression)
        {
            var callExpression = expression as MethodCallExpression;
            if (callExpression == null) return null;

            switch (callExpression.Method.Name)
            {
                case SupportedMethods.ToLower:
                    return ToLower.Create();
                case SupportedMethods.ToUpper:
                    return ToUpper.Create();
                case SupportedMethods.DatePart:
                    return ParseDatePart(callExpression);
                case SupportedMethods.Contains:
                    return ParseContains(callExpression);
                case SupportedMethods.Substring:
                    return ParseSubString(callExpression);
                case SupportedMethods.Trim:
                    return Trim.Create();
                case SupportedMethods.StartsWith:
                    return ParseStartsWith(callExpression);
                case SupportedMethods.EndsWith:
                    return ParseEndsWith(callExpression);
            }

            return null;
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
            var dp = (DatePart)expressionHelper.GetValue(datePartExpression);
            return ValueFunctions.DatePart.Create(dp);
        }

        private IValueFunction ParseSubString(MethodCallExpression expression)
        {
            var arguments = expression.Arguments;
            var startExpression = arguments[0];
            Expression lengthExpression = null;
            if (arguments.Count > 1) lengthExpression = arguments[1];

            var startValue = (int)expressionHelper.GetValue(startExpression);
            if (lengthExpression != null)
            {
                var lengthValue = (int)expressionHelper.GetValue(lengthExpression);
                return Substring.Create(startValue, lengthValue);
            }

            return Substring.Create(startValue);
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
}
