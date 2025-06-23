using System;
using decaf.common;
using decaf.common.Utilities;
using decaf.state;
using decaf.state.QueryTargets;
using FluentAssertions;
using Xunit;

namespace decaf.core_tests;

public class UpdateQueryContextTests
{
    private readonly IUpdateQueryContext context;

    public UpdateQueryContextTests()
    {
        var aliasManager = AliasManager.Create();
        var hashProvider = new HashProvider();
        context = UpdateQueryContext.Create(aliasManager, hashProvider);
    }

    [Fact]
    public void QueryTypeIsUpdate()
    {
        // Act
        var queryType = context.Kind;

        // Assert
        queryType.Should().Be(QueryTypes.Update);
    }

    [Fact]
    public void IdIsCreatedAndNotEmpty()
    {
        // Act
        var id = context.Id;

        // Assert
        id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void UpdateSetsTableCorrectly()
    {
        // Arrange
        var target = TableTarget.Create("users", "u");

        // Act
        context.Update(target);

        // Assert
        context.Table.Should().NotBeNull();
        context.Table.Should().Be(target);
    }

    [Fact]
    public void FromSetsSourceCorrectly()
    {
        // Arrange
        var target = SelectQueryTarget.Create(null, "a");

        // Act
        context.From(target);

        // Assert
        context.Source.Should().NotBeNull();
        context.Source.Should().Be(target);
    }

    [Fact]
    public void SetSucceeds()
    {
        // Arrange
        var column = Column.Create("email", "users", "a");
        var value = state.ValueSources.Update.StaticValueSource.Create(column, typeof(string), "bob@bob.com");

        // Act
        context.Set(value);

        // Assert
        context.Updates.Should().HaveCount(1);
    }

    [Fact]
    public void OutputSucceeds()
    {
        // Arrange
        var column = Column.Create("email", "users", "a");
        var output = Output.Create(column, OutputSources.Updated);

        // Act
        context.Output(output);

        // Assert
        context.Outputs.Should().HaveCount(1);
    }

    [Fact]
    public void WhereClauseSucceeds()
    {
        // Arrange
        var column = Column.Create("id", TableTarget.Create("users", "u"));
        var clause = state.Conditionals.Column.Equals(column, 42);

        // Act
        context.Where(clause);

        // Assert
        context.WhereClause.Should().NotBeNull();
    }
}