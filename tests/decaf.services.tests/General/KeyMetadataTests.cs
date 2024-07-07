using System;
using FluentAssertions;
using Xunit;

namespace decaf.services.tests.General;

public class KeyMetadataTests
{
    [Fact]
    public void KeyMetadataCreationSuccessful()
    {
        // Arrange
        const string name = "ColumnA";

        // Act
        Action method = () => KeyMetadata.Create<int>(name);

        // Assert
        method.Should().NotThrow();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void KeyMetadataCreationShouldThrow(string input)
    {
        // Act
        Action method = () => KeyMetadata.Create<int>(input);

        // Assert
        method.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void KeyMetadataCreationCorrect()
    {
        // Arrange
        const string name = "ColumnA";
        var expectedType = typeof(int);

        // Act
        var metadata = KeyMetadata.Create<int>(name);

        // Assert
        metadata.Should().NotBeNull();
        metadata.Name.Should().Be(name);
        metadata.Type.Should().Be(expectedType);
    }
}