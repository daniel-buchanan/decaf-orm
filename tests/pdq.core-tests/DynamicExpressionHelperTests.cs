using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions;
using pdq.state.Utilities;
using Xunit;

namespace pdq.core_tests
{
	public class DynamicExpressionHelperTests
	{
		private readonly DynamicExpressionHelper helper;

		public DynamicExpressionHelperTests()
		{
			var reflectionHelper = new ReflectionHelper();
			var expressionHelper = new ExpressionHelper(reflectionHelper);
			this.helper = new DynamicExpressionHelper(expressionHelper);
		}

        [Theory]
        [MemberData(nameof(ValidExpressions))]
		public void ParseExpressionDynamicSucceeds(Expression expression, IEnumerable<DynamicPropertyInfo> expected)
        {
			// Act
			var results = this.helper.GetProperties(expression);

			// Assert
			results.Should().BeEquivalentTo(expected);
        }

		public static IEnumerable<object[]> ValidExpressions
        {
			get
            {
				yield return new object[]
				{
					GetDynamicExpr((b) => new
					{
						Name = b.Is("name"),
						City = b.Is("city"),
						Region = b.Is("region_name", "r")
					}),
					new DynamicPropertyInfo[]
                    {
						DynamicPropertyInfo.Create("name", "Name"),
						DynamicPropertyInfo.Create("city", "City"),
						DynamicPropertyInfo.Create("region_name", "Region", alias: "r")
                    }
				};
            }
        }

		public static Expression GetDynamicExpr(Expression<Func<ISelectColumnBuilder, dynamic>> expression)
			=> expression;

        public static Expression GetConcreteExpr<T>(Expression<Func<ISelectColumnBuilder, T>> expression)
            => expression;
    }
}

