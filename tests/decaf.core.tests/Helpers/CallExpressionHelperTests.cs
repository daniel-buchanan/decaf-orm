﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using decaf.common;
using decaf.common.Utilities;
using decaf.common.Utilities.Reflection;
using decaf.common.ValueFunctions;
using decaf.state;
using decaf.state.Conditionals;
using decaf.state.Utilities;
using decaf.tests.common.Models;
using FluentAssertions;
using Xunit;

namespace decaf.core_tests
{
    public class CallExpressionHelperTests
    {
        private readonly IAliasManager aliasManager;
        private readonly IHashProvider hashProvider;
        private readonly CallExpressionHelper helper;

        public CallExpressionHelperTests()
        {
            var reflectionHelper = new ReflectionHelper();
            var expressionHelper = new ExpressionHelper(reflectionHelper);
            this.aliasManager = AliasManager.Create();
            this.hashProvider = new HashProvider();
            var valueFunctionHelper = new ValueFunctionHelper(expressionHelper);
            this.helper = new CallExpressionHelper(expressionHelper, valueFunctionHelper);
        }

        [Fact]
        public void ParseStringContainsConstant()
        {
            // Arrange
            var context = SelectQueryContext.Create(this.aliasManager, this.hashProvider) as IQueryContextExtended;
            Expression<Func<Person, bool>> expr = (p) => p.FirstName.Contains("hello");
            context.AddQueryTarget(expr);

            // Act
            var result = this.helper.ParseExpression(expr, context);

            // Assert
            var values = result as IColumn;
            values.Details.Name.Should().Be(nameof(Person.FirstName));
            values.Details.Source.Alias.Should().Be("p");
            values.Value.Should().Be(true);
            values.ValueFunction.Should().BeEquivalentTo(StringContains.Create("hello"));
            values.EqualityOperator.Should().Be(common.EqualityOperator.Equals);
        }

        [Fact]
        public void ParseStringContainsVariable()
        {
            // Arrange
            var context = SelectQueryContext.Create(this.aliasManager, this.hashProvider) as IQueryContextExtended;
            var str = "hello";
            Expression<Func<Person, bool>> expr = (p) => p.FirstName.Contains(str);
            context.AddQueryTarget(state.QueryTargets.TableTarget.Create(nameof(Person), "p"));

            // Act
            var result = this.helper.ParseExpression(expr, context);

            // Assert
            var values = result as IColumn;
            values.Details.Name.Should().Be(nameof(Person.FirstName));
            values.Details.Source.Alias.Should().Be("p");
            values.Value.Should().Be(true);
            values.ValueFunction.Should().BeEquivalentTo(StringContains.Create(str));
            values.EqualityOperator.Should().Be(common.EqualityOperator.Equals);
        }

        [Fact]
        public void ParseArrayConstantAccessEqualsTrue()
        {
            // Arrange
            var context = SelectQueryContext.Create(this.aliasManager, this.hashProvider) as IQueryContextExtended;
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
        public void ParseTrueEqualsArrayConstantAccess()
        {
            // Arrange
            var context = SelectQueryContext.Create(this.aliasManager, this.hashProvider) as IQueryContextExtended;
            var constantValues = new[] { "hello", "world" };
            Expression<Func<Person, bool>> expr = (p) => true == constantValues.Contains(p.FirstName);
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
            var context = SelectQueryContext.Create(this.aliasManager, this.hashProvider) as IQueryContextExtended;
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
        public void ParseFalseEqualsArrayConstantAccess()
        {
            // Arrange
            var context = SelectQueryContext.Create(this.aliasManager, this.hashProvider) as IQueryContextExtended;
            var constantValues = new[] { "hello", "world" };
            Expression<Func<Person, bool>> expr = (p) => false == constantValues.Contains(p.FirstName);
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
        public void ParseNotEqualsTrueArrayConstantAccess()
        {
            // Arrange
            var context = SelectQueryContext.Create(this.aliasManager, this.hashProvider) as IQueryContextExtended;
            var constantValues = new[] { "hello", "world" };
            Expression<Func<Person, bool>> expr = (p) => true != constantValues.Contains(p.FirstName);
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
            var context = SelectQueryContext.Create(this.aliasManager, this.hashProvider) as IQueryContextExtended;
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
            var context = SelectQueryContext.Create(this.aliasManager, this.hashProvider) as IQueryContextExtended;
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
            var context = SelectQueryContext.Create(this.aliasManager, this.hashProvider) as IQueryContextExtended;
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
        public void ParseSubStringEqualsConstant()
        {
            // Arrange
            var context = SelectQueryContext.Create(this.aliasManager, this.hashProvider) as IQueryContextExtended;
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
        public void ParseConstantEqualsSubString()
        {
            // Arrange
            var context = SelectQueryContext.Create(this.aliasManager, this.hashProvider) as IQueryContextExtended;
            Expression<Func<Person, bool>> expr = (p) => "hello" == p.FirstName.Substring(2);
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
        public void ParseSubStringNotEqualsConstant()
        {
            // Arrange
            var context = SelectQueryContext.Create(this.aliasManager, this.hashProvider) as IQueryContextExtended;
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

        [Fact]
        public void ParseConstantNotEqualsSubString()
        {
            // Arrange
            var context = SelectQueryContext.Create(this.aliasManager, this.hashProvider) as IQueryContextExtended;
            Expression<Func<Person, bool>> expr = (p) => "hello" != p.FirstName.Substring(2);
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

