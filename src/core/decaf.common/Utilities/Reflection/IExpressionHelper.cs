using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using decaf.common;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("decaf")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("decaf.services")]
namespace decaf.common.Utilities.Reflection
{
    public interface IExpressionHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        EqualityOperator ConvertExpressionTypeToEqualityOperator(Expression expression);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        EqualityOperator ConvertExpressionTypeToEqualityOperator(ExpressionType type);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        string GetMethodName(Expression expression);

        /// <summary>
        /// Get the field name from the expression. i.e. p => p.ID (ID would be the field name). Note that this *should* be a SIMPLE expression, as in the previous example.
        /// </summary>
        /// <param name="expression">The expression to get the name from</param>
        /// <returns>The name of the field</returns>
        string GetMemberName(Expression expression);

        /// <summary>
        /// Get the name of the parameter used in an expression. i.e. p => p.ID (p would be the parameter name). Note that this *should* be a SIMPLE expression, as in the previous example.
        /// </summary>
        /// <param name="expression">The expression to get the parameter name from</param>
        /// <returns>The paramter name</returns>
        string GetParameterName(Expression expression);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Type GetMemberType(Expression expression);

        /// <summary>
        /// Get the type of the parameter used in the expression. i.e. p => p.Id (p would be the parameter).
        /// </summary>
        /// <param name="expression">The expression to parse.</param>
        /// <returnsThe type of the expression parameter></returns>
        Type GetParameterType(Expression expression);

        /// <summary>
        /// Get the table/type name
        /// </summary>
        /// <typeparam name="TObject">The type to get the name for</typeparam>
        /// <returns>The name of the type or table</returns>
        string GetTypeName<TObject>();

        /// <summary>
        /// Get the Value from an expression.
        /// </summary>
        /// <param name="expression">The expression to get the value from.</param>
        /// <returns>The value of the expression</returns>
        object GetValue(Expression expression);

        /// <summary>
        /// Checks if the provided expression is a method call on a property.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns></returns>
        bool IsMethodCallOnProperty(Expression expression);

        /// <summary>
        /// Checks if the provided expression is a method call on a constant, or access to a member property.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns></returns>
        bool IsMethodCallOnConstantOrMemberAccess(Expression expression);
    }
}