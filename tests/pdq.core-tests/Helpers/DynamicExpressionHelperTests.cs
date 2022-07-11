using System;
using System.Linq.Expressions;
using FluentAssertions;
using pdq.state.Utilities;
using Xunit;

namespace pdq.core_tests.Helpers
{
    public class DynamicExpressionHelperTests
    {
        private readonly DynamicExpressionHelper dynamicExpressionHelper;

        public DynamicExpressionHelperTests()
        {
            
            var reflectionHelper = new ReflectionHelper();
            var expressionHelper = new ExpressionHelper(reflectionHelper);
            this.dynamicExpressionHelper = new DynamicExpressionHelper(expressionHelper);
        }

        [Fact]
        public void ParseDynamicNoAssignmentSucceeds()
        {
            // Arrange
            Expression<Func<ParamItem, dynamic>> func = (ParamItem p) => new
            {
                p.City,
                p.Age
            };

            // Act
            var properties = this.dynamicExpressionHelper.GetProperties(func);

            // Assert
            properties.Should().HaveCount(2);
            properties.Should().ContainEquivalentOf(DynamicPropertyInfo.Create("Age", type: typeof(ParamItem)));
            properties.Should().ContainEquivalentOf(DynamicPropertyInfo.Create("City", type: typeof(ParamItem)));
        }

        [Fact]
        public void ParseDynamicWithNewNameSucceeds()
        {
            // Arrange
            Expression<Func<ParamItem, dynamic>> func = (ParamItem p) => new
            {
                CityName = p.City,
                p.Age
            };

            // Act
            var properties = this.dynamicExpressionHelper.GetProperties(func);

            // Assert
            properties.Should().HaveCount(2);
            properties.Should().ContainEquivalentOf(DynamicPropertyInfo.Create("Age", type: typeof(ParamItem)));
            properties.Should().ContainEquivalentOf(DynamicPropertyInfo.Create("City", type: typeof(ParamItem), newName: "CityName"));
        }

        [Fact]
        public void ParseConcreteSucceeds()
        {
            // Arrange
            Expression<Func<SourceItem, ParamItem>> func = (SourceItem s) => new ParamItem
            {
                City = s.City,
                Age = s.Age,
                Name = s.FullName
            };

            // Act
            var properties = this.dynamicExpressionHelper.GetProperties(func);

            // Assert
            properties.Should().HaveCount(3);
            properties.Should().ContainEquivalentOf(DynamicPropertyInfo.Create("Age", type: typeof(SourceItem), newName: "Age"));
            properties.Should().ContainEquivalentOf(DynamicPropertyInfo.Create("City", type: typeof(SourceItem), newName: "City"));
            properties.Should().ContainEquivalentOf(DynamicPropertyInfo.Create("FullName", type: typeof(SourceItem), newName: "Name"));
        }

        private class ParamItem
        {
            public string Name { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
            public int Age { get; set; }
        }

        private class SourceItem
        {
            public string FullName { get; set; }
            public string City { get; set; }
            public int Age { get; set; }
        }
    }
}

