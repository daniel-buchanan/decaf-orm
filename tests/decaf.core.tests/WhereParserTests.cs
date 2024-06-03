using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using decaf.common;
using decaf.common.Utilities;
using decaf.common.Utilities.Reflection;
using decaf.common.ValueFunctions;
using decaf.state;
using decaf.state.Conditionals;
using decaf.state.Utilities;
using decaf.state.Utilities.Parsers;
using decaf.tests.common.Models;
using FluentAssertions;
using Xunit;

namespace decaf.core_tests
{
    public class WhereParserTests
    {
        private readonly IAliasManager aliasManager;
        private readonly IHashProvider hashProvider;
        private readonly WhereParser parser;

        public WhereParserTests()
        {
            var reflectionHelper = new ReflectionHelper();
            var expressionHelper = new ExpressionHelper(reflectionHelper);
            this.aliasManager = AliasManager.Create();
            this.hashProvider = new HashProvider();
            var valueFunctionHelper = new ValueFunctionHelper(expressionHelper);
            var callExpressionHelper = new CallExpressionHelper(expressionHelper, valueFunctionHelper);
            var valueParser = new ValueParser(expressionHelper, callExpressionHelper, reflectionHelper);
            var joinParser = new JoinParser(expressionHelper, reflectionHelper);
            this.parser = new WhereParser(expressionHelper, reflectionHelper, callExpressionHelper, joinParser, valueParser);
        }

        [Theory]
        [MemberData(nameof(ValueTests))]
        public void ParseSimpleExpressionSucceeds<T>(
            Expression expression,
            string expectedPropertyName,
            EqualityOperator expectedOperator,
            Func<T> getValue,
            Type functionType)
        {
            // Arrange
            var context = SelectQueryContext.Create(this.aliasManager, this.hashProvider) as IQueryContextExtended;
            context.AddQueryTarget(state.QueryTargets.TableTarget.Create(nameof(Person), "p"));

            // Act
            var result = this.parser.Parse(expression, context);

            // Assert
            var col = result as IColumn;
            if (col == null)
            {
                var not = result as Not;
                col = not.Item as IColumn;
            }

            col.Should().NotBeNull();
            col.Details.Name.Should().Be(expectedPropertyName);
            col.Details.Source.Alias.Should().Be("p");
            col.EqualityOperator.Should().Be(expectedOperator);
            col.Value.Should().Be(getValue());
            if(functionType != null)
            {
                col.ValueFunction.Should().BeAssignableTo(functionType);
            }
        }

        [Fact]
        public void ParseContainsExpressionSucceeds()
        {
            // Arrange
            var context = SelectQueryContext.Create(this.aliasManager, this.hashProvider) as IQueryContextExtended;
            context.AddQueryTarget(state.QueryTargets.TableTarget.Create(nameof(Person), "p"));
            Expression<Func<Person, bool>> expression = (p) => p.FirstName.Contains("smith");

            // Act
            var result = this.parser.Parse(expression, context);

            // Assert
            var col = result as IColumn;
            col.Should().NotBeNull();
            col.Details.Name.Should().Be(nameof(Person.FirstName));
            col.Details.Source.Alias.Should().Be("p");
            col.EqualityOperator.Should().Be(EqualityOperator.Equals);
            col.ValueFunction.Should().BeEquivalentTo(StringContains.Create("smith"));
            col.Value.Should().Be(true);
        }

        [Fact]
        public void ParseNotContainsExpressionSucceeds()
        {
            // Arrange
            var context = SelectQueryContext.Create(this.aliasManager, this.hashProvider) as IQueryContextExtended;
            context.AddQueryTarget(state.QueryTargets.TableTarget.Create(nameof(Person), "p"));
            Expression<Func<Person, bool>> expression = (p) => !p.FirstName.Contains("smith");

            // Act
            var result = this.parser.Parse(expression, context);

            // Assert
            var inversion = result as Not;
            inversion.Should().NotBeNull();
            var col = inversion.Item as IColumn;
            col.Should().NotBeNull();
            col.Details.Name.Should().Be(nameof(Person.FirstName));
            col.Details.Source.Alias.Should().Be("p");
            col.EqualityOperator.Should().Be(EqualityOperator.Equals);
            col.ValueFunction.Should().BeEquivalentTo(StringContains.Create("smith"));
            col.Value.Should().Be(true);
        }

        [Fact]
        public void ParseContainsEqualsFalseExpressionSucceeds()
        {
            // Arrange
            var context = SelectQueryContext.Create(this.aliasManager, this.hashProvider) as IQueryContextExtended;
            context.AddQueryTarget(state.QueryTargets.TableTarget.Create(nameof(Person), "p"));
            Expression<Func<Person, bool>> expression = (p) => p.FirstName.Contains("smith") == false;

            // Act
            var result = this.parser.Parse(expression, context);

            // Assert
            var inversion = result as Not;
            inversion.Should().NotBeNull();
            var col = inversion.Item as IColumn;
            col.Should().NotBeNull();
            col.Details.Name.Should().Be(nameof(Person.FirstName));
            col.Details.Source.Alias.Should().Be("p");
            col.EqualityOperator.Should().Be(EqualityOperator.Equals);
            col.ValueFunction.Should().BeEquivalentTo(StringContains.Create("smith"));
            col.Value.Should().Be(true);
        }

        [Fact]
        public void ParseTrimEqualsValueExpressionSucceeds()
        {
            // Arrange
            var context = SelectQueryContext.Create(this.aliasManager, this.hashProvider) as IQueryContextExtended;
            context.AddQueryTarget(state.QueryTargets.TableTarget.Create(nameof(Person), "p"));
            Expression<Func<Person, bool>> expression = (p) => p.FirstName.Trim() == "bob";

            // Act
            var result = this.parser.Parse(expression, context);

            // Assert
            var col = result as IColumn;
            col.Should().NotBeNull();
            col.Details.Name.Should().Be(nameof(Person.FirstName));
            col.Details.Source.Alias.Should().Be("p");
            col.EqualityOperator.Should().Be(EqualityOperator.Equals);
            col.ValueFunction.Should().BeEquivalentTo(Trim.Create());
            col.Value.Should().Be("bob");
        }

        public static IEnumerable<object[]> ValueTests
        {
            get
            {
                Func<string> getString = () => "john";
                Func<int> getInt = () => 42;
                Func<bool> getFalse = () => false;
                Func<bool> getTrue = () => true;

                yield return new object[]
                {
                    GetExpression<Person>((p) => p.FirstName == "john"),
                    nameof(Person.FirstName),
                    EqualityOperator.Equals,
                    getString,
                    null
                };

                yield return new object[]
                {
                    GetExpression<Person>((p) => p.FirstName != "john"),
                    nameof(Person.FirstName),
                    EqualityOperator.Equals,
                    getString,
                    null
                };

                yield return new object[]
                {
                    GetExpression<Person>((p) => p.AddressId == 42),
                    nameof(Person.AddressId),
                    EqualityOperator.Equals,
                    getInt,
                    null
                };

                yield return new object[]
                {
                    GetExpression<Person>((p) => p.AddressId != 42),
                    nameof(Person.AddressId),
                    EqualityOperator.Equals,
                    getInt,
                    null
                };

                yield return new object[]
                {
                    GetExpression<Person>((p) => p.AddressId > 42),
                    nameof(Person.AddressId),
                    EqualityOperator.GreaterThan,
                    getInt,
                    null
                };

                yield return new object[]
                {
                    GetExpression<Person>((p) => p.AddressId >= 42),
                    nameof(Person.AddressId),
                    EqualityOperator.GreaterThanOrEqualTo,
                    getInt,
                    null
                };

                yield return new object[]
                {
                    GetExpression<Person>((p) => p.AddressId < 42),
                    nameof(Person.AddressId),
                    EqualityOperator.LessThan,
                    getInt,
                    null
                };

                yield return new object[]
                {
                    GetExpression<Person>((p) => p.AddressId <= 42),
                    nameof(Person.AddressId),
                    EqualityOperator.LessThanOrEqualTo,
                    getInt,
                    null
                };

                yield return new object[]
                {
                    GetExpression<Person>(p => p.LastName.Contains("John")),
                    nameof(Person.LastName),
                    EqualityOperator.Equals,
                    getTrue,
                    typeof(StringContains)
                };
            }
        }

        private static Expression GetExpression<T>(Expression<Func<T, bool>> expression)
            => (Expression)expression;
    }
}

