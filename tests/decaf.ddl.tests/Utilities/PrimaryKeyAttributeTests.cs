using decaf.common.Utilities.Reflection;
using decaf.ddl.Attributes;
using FluentAssertions;
using Xunit;

namespace decaf.ddl.Utilities;

public class PrimaryKeyAttributeTests
{
    private readonly AttributeHelper attributeHelper;

    public PrimaryKeyAttributeTests()
    {
        var reflectionHelper = new ReflectionHelper();
        var expressionHelper = new ExpressionHelper(reflectionHelper);
        attributeHelper = new AttributeHelper(reflectionHelper, expressionHelper);
    }
    
    [Fact]
    public void SingleKey_PrimaryKeyParsedSuccessfully()
    {
        // Act
        var pk = attributeHelper.GetPrimaryKey<SingleKeyModel>();

        // Assert
        pk.Columns.Should().Satisfy(cd => cd.Name == nameof(SingleKeyModel.Id));
    }

    [Fact]
    public void SingleKey_PrimaryKeyWithDefaultName()
    {
        // Arrange
        var keyName = $"pk_{nameof(SingleKeyModel)}";
        
        // Act
        var pk = attributeHelper.GetPrimaryKey<SingleKeyModel>();

        // Assert
        pk.Name.Should().Be(keyName);
    }
    
    [Fact]
    public void SingleKey_PrimaryKeyWithProvidedName()
    {
        // Arrange
        var keyName = $"test";
        
        // Act
        var pk = attributeHelper.GetPrimaryKey<SingleKeyModel>(keyName);

        // Assert
        pk.Name.Should().Be(keyName);
    }
    
    [Fact]
    public void MultipleKey_PrimaryKeyParsedSuccessfully()
    {
        // Act
        var pk = attributeHelper.GetPrimaryKey<MultipleKeyModel>();

        // Assert
        pk.Columns.Should().Satisfy(
            cd => cd.Name == nameof(MultipleKeyModel.ComponentOne),
            cd => cd.Name == nameof(MultipleKeyModel.ComponentTwo));
    }

    [Fact]
    public void MultipleKey_PrimaryKeyWithDefaultName()
    {
        // Arrange
        const string keyName = $"pk_{nameof(MultipleKeyModel)}";
        
        // Act
        var pk = attributeHelper.GetPrimaryKey<MultipleKeyModel>();

        // Assert
        pk.Name.Should().Be(keyName);
    }
    
    [Fact]
    public void MultipleKey_PrimaryKeyWithProvidedName()
    {
        // Arrange
        var keyName = $"test";
        
        // Act
        var pk = attributeHelper.GetPrimaryKey<MultipleKeyModel>(keyName);

        // Assert
        pk.Name.Should().Be(keyName);
    }

    private class SingleKeyModel
    {
        [PrimaryKeyComponent]
        public int Id { get; set; }
    }

    private class MultipleKeyModel
    {
        [PrimaryKeyComponent]
        public int ComponentOne { get; set; }
        [PrimaryKeyComponent]
        public int ComponentTwo { get; set; }
    }
}