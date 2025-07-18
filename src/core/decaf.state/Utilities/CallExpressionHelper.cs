﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using decaf.common;
using decaf.common.Utilities;
using decaf.common.Utilities.Reflection;
using decaf.state.Conditionals;

namespace decaf.state.Utilities;

class CallExpressionHelper(
    IExpressionHelper expressionHelper,
    IValueFunctionHelper valueFunctionHelper)
{
    public IWhere? ParseExpression(Expression expression, IQueryContextExtended context)
    {
        Expression expressionToParse;
        if (expression is LambdaExpression lambda)
        {
            var unary = lambda.Body as UnaryExpression;
            if (unary?.NodeType == ExpressionType.Not)
            {
                expressionToParse = Expression.Equal(unary.Operand, Expression.Constant(false));
            }
            else
            {
                expressionToParse = lambda.Body;
            }
        }
        else
        {
            expressionToParse = expression;
        }

        if(expressionHelper.IsMethodCallOnProperty(expressionToParse))
        {
            if (expressionToParse is BinaryExpression binaryExprMethodCall) return ParseBinaryExpression(binaryExprMethodCall, context);
            var constantExpression = Expression.Constant(true);
            binaryExprMethodCall = Expression.Equal(expressionToParse, constantExpression);
            return ParseBinaryExpression(binaryExprMethodCall, context);
        }

        if (!expressionHelper.IsMethodCallOnConstantOrMemberAccess(expressionToParse))
            return ParseBinaryExpression(expressionToParse, context);
        
        if (expressionToParse is not BinaryExpression binaryExpression)
        {
            return ParseMethodAccessCall(expressionToParse, context);
        }

        var call = binaryExpression.Left.CastAs<MethodCallExpression>() ??
                   binaryExpression.Right.CastAs<MethodCallExpression>();
        var constant = binaryExpression.Left.CastAs<ConstantExpression>() ??
                       binaryExpression.Right.CastAs<ConstantExpression>();
        var result = ParseMethodAccessCall(call, context);
        var constantValue = expressionHelper.GetValue<bool>(constant!);

        if (binaryExpression.NodeType == ExpressionType.NotEqual ||
            !constantValue)
            return Not.This(result!);

        return result;
    }

    private IWhere? ParseBinaryExpression(
        Expression expression,
        IQueryContextExtended context)
    {
        GetCallAndNonCallExpressions(
            expression,
            out var callExpr,
            out var nonCallExpr);

        if (callExpr is null) return null;

        var valueFunction = valueFunctionHelper.ParseFunction(callExpr);
        if (valueFunction == null) return null;

        var memberExpression = callExpr.Object?.CastAs<MemberExpression>() ??
                               callExpr.Arguments[0].CastAs<MemberExpression>();

        var op = expressionHelper.ConvertExpressionTypeToEqualityOperator(expression);
        var fieldName = expressionHelper.GetMemberName(memberExpression!);
        var target = context.GetQueryTarget(memberExpression!);
        var col = Column.Create(fieldName!, target!);

        IWhere? result = null;
        var invertResult = op == EqualityOperator.NotEquals;

        if (nonCallExpr?.NodeType == ExpressionType.Constant)
        {
            var value = expressionHelper.GetValue(nonCallExpr);

            if(value is false && op == EqualityOperator.Equals)
            {
                value = invertResult = true;
            }

            var functionType = typeof(Column<>);
            var genericType = value?.GetType() ?? memberExpression?.Type;
            var implementedType = functionType.MakeGenericType(genericType);
            object[] parameters = [col, op, valueFunction, value!];
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public;
            result = (IWhere)Activator.CreateInstance(
                implementedType,
                bindingFlags,
                null,
                parameters,
                System.Globalization.CultureInfo.CurrentCulture);
        }

        if (nonCallExpr?.NodeType == ExpressionType.MemberAccess)
        {
            var fieldB = expressionHelper.GetMemberName(nonCallExpr);
            var targetB = context.GetQueryTarget(nonCallExpr);
            var colB = Column.Create(fieldB!, targetB!);
            result = Conditionals.Column.Equals(col, op, valueFunction, colB);
        }

        if (nonCallExpr?.NodeType == ExpressionType.Call)
        {
            var rightCallExpr = nonCallExpr as MethodCallExpression;
            var rightBody = rightCallExpr?.Object;
            var fieldB = expressionHelper.GetMemberName(rightBody!);
            var targetB = context.GetQueryTarget(rightBody!);
            var colB = Column.Create(fieldB!, targetB!);
            valueFunction = valueFunctionHelper.ParseFunction(rightCallExpr!);

            result = Conditionals.Column.Equals(col, op, valueFunction!, colB);
        }

        return invertResult ? Not.This(result!) : result;
    }

    private static void GetCallAndNonCallExpressions(
        Expression expression,
        out MethodCallExpression? callExpr,
        out Expression? nonCallExpr)
    {
        callExpr = null;
        nonCallExpr = null;

        var binaryExpr = expression.CastAs<BinaryExpression>();
        if (binaryExpr is null)
        {
            var lambda = expression.CastAs<LambdaExpression>();
            binaryExpr = lambda?.Body.CastAs<BinaryExpression>();
        }

        if (binaryExpr is null) return;

        var left = binaryExpr.Left;
        var right = binaryExpr.Right;

        if (left.NodeType == ExpressionType.Call)
        {
            callExpr = left as MethodCallExpression;
            nonCallExpr = right;
        }
        else if (right.NodeType == ExpressionType.Call)
        {
            callExpr = right as MethodCallExpression;
            nonCallExpr = left;
        }
    }

    private IWhere? ParseMethodAccessCall(Expression? expression, IQueryContextExtended context)
    {
        var callExpression = expression?.CastAs<MethodCallExpression>();
        if (callExpression is null) return null;

        IWhere? result = null;
        switch(callExpression.Method.Name)
        {
            case SupportedMethods.Contains:
                result = ParseVariableContainsAsValuesIn(callExpression, context);
                break;
        }

        return result;
    }

    private IWhere? ParseVariableContainsAsValuesIn(MethodCallExpression call, IQueryContextExtended context)
    {
        var arg = call.Arguments[0];
        if (arg.NodeType != ExpressionType.MemberAccess) return null;

        MemberExpression? memberAccessExp;
        Expression? valueAccessExp;

        // check if this is a list or contains with variable
        if (call.Arguments.Count != 1)
        {
            // if the underlying value is an array
            memberAccessExp = call.Arguments[1].CastAs<MemberExpression>();
            valueAccessExp = call.Arguments[0];
        }
        else
        {
            // check for passed in variable
            memberAccessExp = call.Arguments[0].CastAs<MemberExpression>();
            valueAccessExp = call.Object;
        }

        if (memberAccessExp is null) return null;

        // get values
        var valueMember = valueAccessExp?.CastAs<MemberExpression>();
        var values = expressionHelper.GetValue(valueMember!);

        // get alias and field name
        var fieldName = expressionHelper.GetMemberName(memberAccessExp);

        // setup arguments
        var typeArgs = new[] { memberAccessExp.Type };

        var inputGenericType = typeof(List<>);
        var inputType = inputGenericType.MakeGenericType(typeArgs);
        var input = Activator.CreateInstance(inputType, values);
        var target = context.GetQueryTarget(memberAccessExp);
        var col = Column.Create(fieldName!, target!);

        object[] parameters = [col, input];
        var enumerableGenericType = typeof(IEnumerable<>);
        var enumerableType = enumerableGenericType.MakeGenericType(typeArgs);

        // get generic type
        var toCreate = typeof(InValues<>);
        var genericToCreate = toCreate.MakeGenericType(typeArgs);
        var parameterTypes = new[] { col.GetType(), enumerableType };
        var bindingFlags = BindingFlags.Public | BindingFlags.Instance;

        var ctor = genericToCreate.GetConstructor(bindingFlags, null, parameterTypes, null);

        if (ctor == null) return null;

        // return instance
        return (IWhere)ctor.Invoke(parameters);
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