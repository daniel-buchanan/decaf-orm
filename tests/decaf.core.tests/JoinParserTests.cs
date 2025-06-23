using System;
using System.Linq.Expressions;
using decaf.common;
using decaf.common.Utilities;
using decaf.common.Utilities.Reflection;
using decaf.state;
using decaf.state.Conditionals;
using decaf.state.Utilities;
using decaf.state.Utilities.Parsers;
using decaf.tests.common.Models;
using FluentAssertions;
using Xunit;

namespace decaf.core_tests;

public class JoinParserTests
{
    private readonly IAliasManager aliasManager;
    private readonly IHashProvider hashProvider;
    private readonly JoinParser parser;

    public JoinParserTests()
    {
        var reflectionHelper = new ReflectionHelper();
        var expressionHelper = new ExpressionHelper(reflectionHelper);
        aliasManager = AliasManager.Create();
        hashProvider = new HashProvider();
        var valueFunctionHelper = new ValueFunctionHelper(expressionHelper);
        var callExpressionHelper = new CallExpressionHelper(expressionHelper, valueFunctionHelper);
        var valueParser = new ValueParser(expressionHelper, callExpressionHelper, reflectionHelper);
        var joinParser = new JoinParser(expressionHelper, reflectionHelper);
        parser = new JoinParser(expressionHelper, reflectionHelper);
    }

    [Fact]
    public void ParseSimpleJoinSucceeds()
    {
        // Arrange
        var context = SelectQueryContext.Create(aliasManager, hashProvider) as IQueryContextExtended;
        context.AddQueryTarget(state.QueryTargets.TableTarget.Create(nameof(Person), "p"));
        Expression<Func<Person, Address, bool>> expression = (p, a) => p.AddressId == a.Id;

        // Act
        var result = parser.Parse(expression, context);

        // Assert
        result.Should().NotBeNull();
        var match = result as ColumnMatch;
        match.Left.Should().NotBeNull();
        match.Left.Name.Should().Be(nameof(Person.AddressId));
        match.Left.Source.Alias.Should().Be("p");
        match.Right.Should().NotBeNull();
        match.Right.Name.Should().Be(nameof(Address.Id));
        match.Right.Source.Alias.Should().Be("a");
    }

    [Fact]
    public void ParseReverseSimpleJoinSucceeds()
    {
        // Arrange
        var context = SelectQueryContext.Create(aliasManager, hashProvider) as IQueryContextExtended;
        context.AddQueryTarget(state.QueryTargets.TableTarget.Create(nameof(Person), "p"));
        Expression<Func<Address, Person, bool>> expression = (a, p) => a.Id == p.AddressId;

        // Act
        var result = parser.Parse(expression, context);

        // Assert
        result.Should().NotBeNull();
        var match = result as ColumnMatch;
        match.Right.Should().NotBeNull();
        match.Right.Name.Should().Be(nameof(Person.AddressId));
        match.Right.Source.Alias.Should().Be("p");
        match.Left.Should().NotBeNull();
        match.Left.Name.Should().Be(nameof(Address.Id));
        match.Left.Source.Alias.Should().Be("a");
    }
}