using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using decaf.common;
using decaf.common.Utilities.Reflection;
using decaf.common.ValueFunctions;
using decaf.tests.common.Models;
using FluentAssertions;
using Xunit;

namespace decaf.core_tests.Helpers;

public class ValueFunctionHelperTests
{
    private readonly IValueFunctionHelper helper;
    
    public ValueFunctionHelperTests()
    {
        var reflectionHelper = new ReflectionHelper();
        var expressionHelper = new ExpressionHelper(reflectionHelper);
        helper = new ValueFunctionHelper(expressionHelper);
    }

    [Theory]
    [MemberData(nameof(FunctionCalls))]
    private void ParseFunction<TResult>(Expression<Func<string, object>> expression, ValueFunction functionType,
        Func<IValueFunction, bool> additionalVerification)
    {
        // Arrange 
        var expr = expression.Body;
        if (expr is UnaryExpression unaryExpression)
            expr = unaryExpression.Operand as MethodCallExpression;
        
        // Act 
        var func = helper.ParseFunction(expr);
        
        // Assert
        func.Should().NotBeNull();
        func.Type.Should().Be(functionType);
        var additionalVerifications = additionalVerification(func);
        additionalVerifications.Should().BeTrue();
    }
    
    public static TheoryData<Expression<Func<string, object>>, ValueFunction, Func<IValueFunction, bool>> FunctionCalls = 
        new ()
        {
            {p => p.ToLower(), ValueFunction.ToLower, f => true},
            {p => p.ToUpper(), ValueFunction.ToUpper, f => true},
            {p => p.Contains("hello"), ValueFunction.Contains, f =>
            {
                var x = f as StringContains;
                return x.Value == "hello";
            }},
            {p => p.StartsWith("hello"), ValueFunction.StartsWith, f =>
            {
                var x = f as StringStartsWith;
                return x.Value == "hello";
            }},
            {p => p.EndsWith("hello"), ValueFunction.EndsWith, f =>
            {
                var x = f as StringEndsWith;
                return x.Value == "hello";
            }},
            {p => p.Substring(0), ValueFunction.Substring, f =>
            {
                var x = f as Substring;
                var start = (int)x.Arguments[0];
                return start == 0;
            }},
            {p => p.Substring(0, 10), ValueFunction.Substring, f =>
            {
                var x = f as Substring;
                var start = (int)x.Arguments[0];
                var end   = (int)x.Arguments[1];
                return start == 0 && end == 10;
            }}
        };
} 