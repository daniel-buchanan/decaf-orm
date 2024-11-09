using decaf.state;
using decaf.state.Ddl.Definitions;
using decaf.tests.common.Models;
using FluentAssertions;
using Xunit;

namespace decaf.ddl.Implementation;

public partial class CreateTableTests
{
    [Fact]
    public void GenericColumnsSucceeds()
    {
        // Arrange
        var impl = query.CreateTable();
        
        // Act
        impl.Named(TableName)
            .WithColumns<Note>();

        // Assert
        var o = query.Context as ICreateTableQueryContext;
        o!.Columns.Should().ContainEquivalentOf(ColumnDefinition.Create(nameof(Note.PersonId), typeof(int)));
        o!.Columns.Should().ContainEquivalentOf(ColumnDefinition.Create(nameof(Note.Value), typeof(string)));
    }
    
    [Fact]
    public void FromTypeSucceeds()
    {
        // Arrange
        var impl = query.CreateTable();
        
        // Act
        impl.FromType<Note>();

        // Assert
        var o = query.Context as ICreateTableQueryContext;
        o!.Name.Should().Be(nameof(Note));
        o!.Columns.Should().ContainEquivalentOf(ColumnDefinition.Create(nameof(Note.PersonId), typeof(int)));
        o!.Columns.Should().ContainEquivalentOf(ColumnDefinition.Create(nameof(Note.Value), typeof(string)));
    }
    
    [Fact]
    public void TypedColumnsSucceeds()
    {
        // Arrange
        var impl = query.CreateTable<Note>();
        
        // Act
        impl.WithColumns(c => c.PersonId, c => c.Value);

        // Assert
        var o = query.Context as ICreateTableQueryContext;
        o!.Name.Should().Be(nameof(Note));
        o!.Columns.Should().ContainEquivalentOf(ColumnDefinition.Create(nameof(Note.PersonId), typeof(int)));
        o!.Columns.Should().ContainEquivalentOf(ColumnDefinition.Create(nameof(Note.Value), typeof(string)));
    }
}