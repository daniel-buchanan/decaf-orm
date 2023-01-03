using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions;
using pdq.common;
using pdq.common.Utilities;
using pdq.core_tests.Models;
using pdq.state;
using pdq.state.Conditionals;
using pdq.state.Utilities;
using pdq.state.Utilities.Parsers;
using Xunit;

namespace pdq.core_tests
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
            var callExpressionHelper = new CallExpressionHelper(expressionHelper);
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
            var context = SelectQueryContext.Create(this.aliasManager, this.hashProvider) as IQueryContextInternal;
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
            var context = SelectQueryContext.Create(this.aliasManager, this.hashProvider) as IQueryContextInternal;

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

        private static Expression GetExpression<T>(Expression<Func<T, bool>> expression)
            => (Expression)expression;
    }
}

