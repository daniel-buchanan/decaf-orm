using decaf.common.Utilities.Reflection.Dynamic;
using FluentAssertions;
using Xunit;

namespace decaf.ddl.Utilities
{
    public class ColumnDefinitionBuilderTests
    {
        [Fact]
        public void FromEmptyDynamicColumnInfoSucceeds()
        {
            // Arrange
            var info = DynamicColumnInfo.Empty();

            // Act
            var def = ColumnDefinitionBuilder.Build(info);

            // Assert
            def.Name.Should().BeNull();
        }
    }
}