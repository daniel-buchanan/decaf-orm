using System;
using System.Linq.Expressions;
using pdq.common;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("pdq")]
namespace pdq.state.Utilities
{
    internal interface IExpressionHelper
    {
        EqualityOperator ConvertExpressionTypeToEqualityOperator(ExpressionType type);
        string GetMethodName(Expression expression);
        string GetName(Expression expression);
        string GetParameterName(Expression expr);
        Type GetType(Expression expression);
        string GetTypeName<TObject>();
        object GetValue(Expression expression);
        IWhere ParseWhereExpression(Expression expr);
        IWhere ParseWhere(Expression expr, bool excludeAlias);
    }
}