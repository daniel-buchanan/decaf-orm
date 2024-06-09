using System;
using System.Collections.Generic;
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

namespace decaf.core_tests
{
    public class ValueParserTests
    {
        private readonly IAliasManager aliasManager;
        private readonly IHashProvider hashProvider;
        private readonly ValueParser parser;

        public ValueParserTests()
        {
            var reflectionHelper = new ReflectionHelper();
            var expressionHelper = new ExpressionHelper(reflectionHelper);
            this.aliasManager = AliasManager.Create();
            this.hashProvider = new HashProvider();
            var valueFunctionHelper = new ValueFunctionHelper(expressionHelper);
            var callExpressionHelper = new CallExpressionHelper(expressionHelper, valueFunctionHelper);
            this.parser = new ValueParser(expressionHelper, callExpressionHelper, reflectionHelper);
        }

        [Theory]
        [MemberData(nameof(ValueTests))]
        public void ParseExpressionSucceeds<T>(
            Expression expression,
            string expectedPropertyName,
            EqualityOperator expectedOperator,
            Func<T> getValue)
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
        }

        [Theory]
        [MemberData(nameof(ValueTests))]
        public void ParseExpressionNoExistingTargetSucceeds<T>(
            Expression expression,
            string expectedPropertyName,
            EqualityOperator expectedOperator,
            Func<T> getValue)
        {
            // Arrange
            var context = SelectQueryContext.Create(this.aliasManager, this.hashProvider) as IQueryContextExtended;

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
        }

        public static IEnumerable<object[]> ValueTests
        {
            get
            {
                Func<string> getString = () => "john";
                Func<int> getInt = () => 42;

                yield return new object[]
                {
                    GetExpression<Person>((p) => p.FirstName == "john"),
                    nameof(Person.FirstName),
                    EqualityOperator.Equals,
                    getString
                };

                yield return new object[]
                {
                    GetExpression<Person>((p) => "john" == p.FirstName),
                    nameof(Person.FirstName),
                    EqualityOperator.Equals,
                    getString
                };

                yield return new object[]
                {
                    GetExpression<Person>((p) => p.FirstName != "john"),
                    nameof(Person.FirstName),
                    EqualityOperator.Equals,
                    getString
                };

                yield return new object[]
                {
                    GetExpression<Person>((p) => p.AddressId == 42),
                    nameof(Person.AddressId),
                    EqualityOperator.Equals,
                    getInt
                };

                yield return new object[]
                {
                    GetExpression<Person>((p) => p.AddressId != 42),
                    nameof(Person.AddressId),
                    EqualityOperator.Equals,
                    getInt
                };

                yield return new object[]
                {
                    GetExpression<Person>((p) => p.AddressId > 42),
                    nameof(Person.AddressId),
                    EqualityOperator.GreaterThan,
                    getInt
                };

                yield return new object[]
                {
                    GetExpression<Person>((p) => p.AddressId >= 42),
                    nameof(Person.AddressId),
                    EqualityOperator.GreaterThanOrEqualTo,
                    getInt
                };

                yield return new object[]
                {
                    GetExpression<Person>((p) => p.AddressId < 42),
                    nameof(Person.AddressId),
                    EqualityOperator.LessThan,
                    getInt
                };

                yield return new object[]
                {
                    GetExpression<Person>((p) => p.AddressId <= 42),
                    nameof(Person.AddressId),
                    EqualityOperator.LessThanOrEqualTo,
                    getInt
                };
            }
        }

        private static Expression<Func<T, bool>> GetExpression<T>(Expression<Func<T, bool>> expression)
            => expression;
    }
}

