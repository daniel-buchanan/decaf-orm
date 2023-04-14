using System;
using System.Collections.Generic;
using FluentAssertions;
using pdq.common.Utilities.Reflection;
using Xunit;

namespace pdq.core_tests.Helpers
{
	public class ReflectionHelperTests
	{
		private readonly IReflectionHelper reflectionHelper;
		public ReflectionHelperTests()
		{
			this.reflectionHelper = new ReflectionHelper();
		}

		[Fact]
		public void GetPropertyValue_Dyanmic()
		{
			// Arrange
			var obj = new
			{
				prop = "hello"
			};

			// Act
			var val = reflectionHelper.GetPropertyValue(obj, "prop");

			// Assert
			val.Should().Be("hello");
		}

		[Theory]
		[MemberData(nameof(DynamicMemberTypes))]
		public void GetMemberType_Dynamic(dynamic obj, string propName, Type expected)
		{
			// Act
			var val = reflectionHelper.GetMemberType(obj, propName);

			// Assert
			expected.Should().Be(val);
		}

		public static IEnumerable<object[]> DynamicMemberTypes
		{
			get
			{
				yield return new object[] { new { prop = "hello" }, "prop", typeof(string) };
                yield return new object[] { new { prop = 42 }, "prop", typeof(int) };
            }
		}
	}
}

