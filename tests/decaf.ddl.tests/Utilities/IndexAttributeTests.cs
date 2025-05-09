using System.Linq;
using decaf.common.Utilities.Reflection;
using decaf.ddl.Attributes;
using FluentAssertions;
using Xunit;

namespace decaf.ddl.Utilities;

public class IndexAttributeTests
{
    private readonly AttributeHelper attributeHelper;

    public IndexAttributeTests()
    {
        var reflectionHelper = new ReflectionHelper();
        var expressionHelper = new ExpressionHelper(reflectionHelper);
        attributeHelper = new AttributeHelper(reflectionHelper, expressionHelper);
    }
    
    [Fact]
    public void SingleKeyIndex_ParsedSuccessfully()
    {
        // Act
        var indexes = attributeHelper.GetIndexes<SingleKeyModel>();

        // Assert
        indexes.Should().Satisfy(idx => idx.Name == "idx");
    }
    
    [Fact]
    public void SingleIndexDefined_ParsedSuccessfully()
    {
        // Act
        var indexes = attributeHelper.GetIndexes<SingleIndexWithComponentsModel>();

        // Assert
        indexes.Should().Satisfy(idx => idx.Name == "idx1" && idx.Columns.Count() == 2);
    }
    
    [Fact]
    public void MultipleIndexDefined_ParsedSuccessfully()
    {
        // Act
        var indexes = attributeHelper.GetIndexes<MultipleIndexModel>();

        // Assert
        indexes.Should().Satisfy(
            idx => idx.Name == "idx1" && idx.Columns.Count() == 2,
            idx => idx.Name == "idx2" && idx.Columns.Count() == 1);
    }

    [Index("idx")]
    private class SingleKeyModel
    {
        [PrimaryKeyComponent]
        [IndexComponent("idx")]
        public int Id { get; set; }
    }

    [Index("idx1")]
    [Index("idx2")]
    private class SingleIndexWithComponentsModel
    {
        [IndexComponent("idx1")]
        public int ComponentOne { get; set; }
        [IndexComponent("idx1")]
        public int ComponentTwo { get; set; }
    }
    
    [Index("idx1")]
    [Index("idx2")]
    private class MultipleIndexModel
    {
        [IndexComponent("idx1")]
        public int ComponentOne { get; set; }
        [IndexComponent("idx1")]
        public int ComponentTwo { get; set; }
        [IndexComponent("idx2")]
        public int ComponentThree { get; set; }
    }
}