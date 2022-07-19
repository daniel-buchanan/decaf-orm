using System;
using System.Linq.Expressions;
using FluentAssertions;
using pdq.common;
using pdq.core_tests.Models;
using pdq.state;
using pdq.state.Conditionals;
using pdq.state.Utilities;
using Xunit;

namespace pdq.core_tests
{
    public class CallExpressionHelperTests
    {
        private readonly IAliasManager aliasManager;
        private readonly CallExpressionHelper helper;

        public CallExpressionHelperTests()
        {
            var reflectionHelper = new ReflectionHelper();
            var expressionHelper = new ExpressionHelper(reflectionHelper);
            this.aliasManager = AliasManager.Create();
            this.helper = new CallExpressionHelper(expressionHelper, reflectionHelper);
        }

        [Fact]
        public void ParseStringContains()
        {
            // Arrange
            var context = SelectQueryContext.Create(this.aliasManager) as IQueryContextInternal;
            Expression<Func<Person, bool>> expr = (p) => p.FirstName.Contains("hello");
            context.AddQueryTarget(expr);

            // Act
            var result = this.helper.ParseExpression(expr, context);

            // Assert
            var values = result as IColumn;
            values.Details.Name.Should().Be(nameof(Person.FirstName));
            values.Details.Source.Alias.Should().Be("p");
            values.Value.Should().Be("hello");
            values.EqualityOperator.Should().Be(common.EqualityOperator.Like);
        }
    }
}

