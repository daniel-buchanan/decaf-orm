using System;
using System.Linq.Expressions;
using FluentAssertions;
using pdq.common;
using pdq.core_tests.Models;
using pdq.state.Conditionals;
using pdq.state.Utilities;
using Xunit;

namespace pdq.core_tests
{
    public class CallExpressionHelperTests
    {
        private readonly CallExpressionHelper helper;

        public CallExpressionHelperTests()
        {
            var reflectionHelper = new ReflectionHelper();
            var expressionHelper = new ExpressionHelper(reflectionHelper);
            var aliasManager = AliasManager.Create();
            this.helper = new CallExpressionHelper(expressionHelper, reflectionHelper, aliasManager);
        }

        [Fact]
        public void ParseStringContains()
        {
            // Arrange
            Expression<Func<Person, bool>> expr = (p) => p.FirstName.Contains("hello");

            // Act
            var result = this.helper.ParseExpression(expr);

            // Assert
            var values = result as IColumn;
            values.Details.Name.Should().Be(nameof(Person.FirstName));
            values.Details.Source.Alias.Should().Be("p");
            values.Value.Should().Be("hello");
            values.EqualityOperator.Should().Be(common.EqualityOperator.Like);
        }
    }
}

