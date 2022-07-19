using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions;
using pdq.common;
using pdq.core_tests.Models;
using pdq.state.Conditionals;
using pdq.state.Utilities;
using pdq.state.Utilities.Parsers;
using Xunit;

namespace pdq.core_tests
{
    public class ValueParserTests
    {
        private readonly ValueParser parser;

        public ValueParserTests()
        {
            var reflectionHelper = new ReflectionHelper();
            var expressionHelper = new ExpressionHelper(reflectionHelper);
            var aliasManager = AliasManager.Create();
            var callExpressionHelper = new CallExpressionHelper(expressionHelper, reflectionHelper, aliasManager);
            this.parser = new ValueParser(expressionHelper, callExpressionHelper, reflectionHelper, aliasManager);
        }

        [Theory]
        [MemberData(nameof(ValueTests))]
        public void ParseExpressionSucceeds<T>(
            Expression expression,
            string expectedPropertyName,
            EqualityOperator expectedOperator,
            Func<T> getValue)
        {
            // Act
            var result = this.parser.Parse(expression);

            // Assert
            var col = result as IColumn;
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
                    GetExpression<Person>((p) => p.FirstName != "john"),
                    nameof(Person.FirstName),
                    EqualityOperator.NotEquals,
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
                    EqualityOperator.NotEquals,
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

