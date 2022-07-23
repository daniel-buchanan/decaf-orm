using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using pdq.common;
using pdq.core_tests.Models;
using pdq.state;
using pdq.state.Conditionals;
using pdq.state.Conditionals.ValueFunctions;
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
            this.helper = new CallExpressionHelper(expressionHelper);
        }

        [Fact]
        public void ParseStringContainsConstant()
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

        [Fact]
        public void ParseStringContainsVariable()
        {
            // Arrange
            var context = SelectQueryContext.Create(this.aliasManager) as IQueryContextInternal;
            var str = "hello";
            Expression<Func<Person, bool>> expr = (p) => p.FirstName.Contains(str);
            context.AddQueryTarget(state.QueryTargets.TableTarget.Create(nameof(Person), "p"));

            // Act
            var result = this.helper.ParseExpression(expr, context);

            // Assert
            var values = result as IColumn;
            values.Details.Name.Should().Be(nameof(Person.FirstName));
            values.Details.Source.Alias.Should().Be("p");
            values.Value.Should().Be("hello");
            values.EqualityOperator.Should().Be(common.EqualityOperator.Like);
        }

        [Fact]
        public void ParseArrayConstantAccessEqualsTrue()
        {
            // Arrange
            var context = SelectQueryContext.Create(this.aliasManager) as IQueryContextInternal;
            var constantValues = new[] { "hello", "world" };
            Expression<Func<Person, bool>> expr = (p) => constantValues.Contains(p.FirstName) == true;
            context.AddQueryTarget(state.QueryTargets.TableTarget.Create(nameof(Person), "p"));

            // Act
            var result = this.helper.ParseExpression(expr, context);

            // Assert
            var values = result as IInValues;
            values.Column.Name.Should().Be(nameof(Person.FirstName));
            values.Column.Source.Alias.Should().Be("p");
            values.GetValues().Should().BeEquivalentTo(constantValues);
        }

        [Fact]
        public void ParseArrayConstantAccessEqualsFalse()
        {
            // Arrange
            var context = SelectQueryContext.Create(this.aliasManager) as IQueryContextInternal;
            var constantValues = new[] { "hello", "world" };
            Expression<Func<Person, bool>> expr = (p) => constantValues.Contains(p.FirstName) == false;
            context.AddQueryTarget(state.QueryTargets.TableTarget.Create(nameof(Person), "p"));

            // Act
            var result = this.helper.ParseExpression(expr, context);

            // Assert
            var inversion = result as Not;
            inversion.Should().NotBeNull();
            var values = inversion.Item as IInValues;
            values.Column.Name.Should().Be(nameof(Person.FirstName));
            values.Column.Source.Alias.Should().Be("p");
            values.GetValues().Should().BeEquivalentTo(constantValues);
        }

        [Fact]
        public void ParseArrayConstantAccessNotEqualsTrue()
        {
            // Arrange
            var context = SelectQueryContext.Create(this.aliasManager) as IQueryContextInternal;
            var constantValues = new[] { "hello", "world" };
            Expression<Func<Person, bool>> expr = (p) => constantValues.Contains(p.FirstName) != true;
            context.AddQueryTarget(state.QueryTargets.TableTarget.Create(nameof(Person), "p"));

            // Act
            var result = this.helper.ParseExpression(expr, context);

            // Assert
            var inversion = result as Not;
            inversion.Should().NotBeNull();
            var values = inversion.Item as IInValues;
            values.Column.Name.Should().Be(nameof(Person.FirstName));
            values.Column.Source.Alias.Should().Be("p");
            values.GetValues().Should().BeEquivalentTo(constantValues);
        }

        [Fact]
        public void ParseArrayConstantAccess()
        {
            // Arrange
            var context = SelectQueryContext.Create(this.aliasManager) as IQueryContextInternal;
            var constantValues = new[] { "hello", "world" };
            Expression<Func<Person, bool>> expr = (p) => constantValues.Contains(p.FirstName);
            context.AddQueryTarget(state.QueryTargets.TableTarget.Create(nameof(Person), "p"));

            // Act
            var result = this.helper.ParseExpression(expr, context);

            // Assert
            var values = result as IInValues;
            values.Column.Name.Should().Be(nameof(Person.FirstName));
            values.Column.Source.Alias.Should().Be("p");
            values.GetValues().Should().BeEquivalentTo(constantValues);
        }

        [Fact]
        public void ParseEnumerableConstantAccess()
        {
            // Arrange
            var context = SelectQueryContext.Create(this.aliasManager) as IQueryContextInternal;
            var constantValues = (new List<string> { "hello", "world" }).AsEnumerable();
            Expression<Func<Person, bool>> expr = (p) => constantValues.Contains(p.FirstName);
            context.AddQueryTarget(state.QueryTargets.TableTarget.Create(nameof(Person), "p"));

            // Act
            var result = this.helper.ParseExpression(expr, context);

            // Assert
            var values = result as IInValues;
            values.Column.Name.Should().Be(nameof(Person.FirstName));
            values.Column.Source.Alias.Should().Be("p");
            values.GetValues().Should().BeEquivalentTo(constantValues);
        }

        [Fact]
        public void ParseListConstantAccess()
        {
            // Arrange
            var context = SelectQueryContext.Create(this.aliasManager) as IQueryContextInternal;
            var constantValues = new List<string> { "hello", "world" };
            Expression<Func<Person, bool>> expr = (p) => constantValues.Contains(p.FirstName);
            context.AddQueryTarget(state.QueryTargets.TableTarget.Create(nameof(Person), "p"));

            // Act
            var result = this.helper.ParseExpression(expr, context);

            // Assert
            var values = result as IInValues;
            values.Column.Name.Should().Be(nameof(Person.FirstName));
            values.Column.Source.Alias.Should().Be("p");
            values.GetValues().Should().BeEquivalentTo(constantValues);
        }

        [Fact]
        public void ParseSubString()
        {
            // Arrange
            var context = SelectQueryContext.Create(this.aliasManager) as IQueryContextInternal;
            Expression<Func<Person, bool>> expr = (p) => p.FirstName.Substring(2) == "hello";
            context.AddQueryTarget(state.QueryTargets.TableTarget.Create(nameof(Person), "p"));

            // Act
            var result = this.helper.ParseExpression(expr, context);

            // Assert
            var col = result as IColumn;
            col.Details.Name.Should().Be(nameof(Person.FirstName));
            col.Details.Source.Alias.Should().Be("p");
            col.ValueFunction.Should().BeEquivalentTo(Substring.Create(2));
            col.Value.Should().Be("hello");
        }

        [Fact]
        public void ParseSubStringNotEquals()
        {
            // Arrange
            var context = SelectQueryContext.Create(this.aliasManager) as IQueryContextInternal;
            Expression<Func<Person, bool>> expr = (p) => p.FirstName.Substring(2) != "hello";
            context.AddQueryTarget(state.QueryTargets.TableTarget.Create(nameof(Person), "p"));

            // Act
            var result = this.helper.ParseExpression(expr, context);

            // Assert
            var inversion = result as Not;
            inversion.Should().NotBeNull();
            var col = inversion.Item as IColumn;
            col.Details.Name.Should().Be(nameof(Person.FirstName));
            col.Details.Source.Alias.Should().Be("p");
            col.ValueFunction.Should().BeEquivalentTo(Substring.Create(2));
            col.Value.Should().Be("hello");
        }
    }
}

