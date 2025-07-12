using System;
using System.Linq.Expressions;

namespace decaf.common.Utilities.Reflection;

static class MemberAccess
{
    public static object? GetValue(Expression expression)
    {
        var objectMember = Expression.Convert(expression, expression.Type);
        var getterLambda = Expression.Lambda(objectMember);

        try
        {
            var getter = getterLambda.Compile();

            return getter.DynamicInvoke();
        }
        catch
        {
            return null;
        }
    }

    public static Type GetMemberType(Expression expression)
    {
        return ((MemberExpression)expression).Type;
    }

    public static Type GetParameterType(Expression expression)
    {
        var memberExpression = expression.CastAs<MemberExpression>();
        if(memberExpression is null)
            throw new InvalidOperationException("Unable to cast expression to MemberExpression.");
        return memberExpression.Member.DeclaringType!;
    }

    public static string GetName(Expression expression, IReflectionHelper helper)
    {
        var memberExpr = (MemberExpression)expression;
        return helper.GetFieldName(memberExpr.Member);
    }
}