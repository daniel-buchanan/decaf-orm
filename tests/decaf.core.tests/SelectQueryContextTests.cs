using System;
using System.Collections.Generic;
using decaf.common;
using decaf.common.Utilities;
using decaf.state;
using decaf.state.QueryTargets;
using FluentAssertions;
using Xunit;

namespace decaf.core_tests;

public class SelectQueryContextTests
{
    private readonly ISelectQueryContext context;

    public SelectQueryContextTests()
    {
        var aliasManager = AliasManager.Create();
        var hashProvider = new HashProvider();
        context = SelectQueryContext.Create(aliasManager, hashProvider);
    }

    [Fact]
    public void QueryTypeIsSelect()
    {
        // Act
        var queryType = context.Kind;

        // Assert
        queryType.Should().Be(QueryTypes.Select);
    }

    [Fact]
    public void IdIsCreatedAndNotEmpty()
    {
        // Act
        var id = context.Id;

        // Assert
        id.Should().NotBe(Guid.Empty);
    }

    [Theory]
    [MemberData(nameof(ColumnTests))]
    public void AddColumnSucceeds(Column column)
    {
        // Act
        context.Select(column);

        // Assert
        context.Columns.Count.Should().Be(1);
    }

    [Theory]
    [MemberData(nameof(ColumnTests))]
    public void AddExistingColumnDoesNothing(Column column)
    {
        // Arrange
        context.Select(column);

        // Act
        context.Select(column);

        // Assert
        context.Columns.Count.Should().Be(1);
    }

    [Theory]
    [MemberData(nameof(TableTests))]
    public void SelectFromSucceeds(ITableTarget target)
    {
        // Act
        context.From(target);

        // Assert
        context.QueryTargets.Count.Should().Be(1);
    }

    [Theory]
    [MemberData(nameof(TableTests))]
    public void SelectFromSameDoesNothing(ITableTarget target)
    {
        // Arrange
        context.From(target);

        // Act
        context.From(target);

        // Assert
        context.QueryTargets.Count.Should().Be(1);
    }

    [Fact]
    public void AddGroupBySucceeds()
    {
        // Arrange
        var group = GroupBy.Create("name", TableTarget.Create("users", "u"));

        // Act
        context.GroupBy(group);

        // Assert
        context.GroupByClauses.Count.Should().Be(1);
    }

    [Fact]
    public void AddSameGroupByFails()
    {
        // Arrange
        var group = GroupBy.Create("name", TableTarget.Create("users", "u"));

        // Act
        context.GroupBy(group);
        context.GroupBy(group);

        // Assert
        context.GroupByClauses.Count.Should().Be(1);
    }

    [Fact]
    public void AddOrderBySucceeds()
    {
        // Arrange
        var order = OrderBy.Create("name", TableTarget.Create("users", "u"), SortOrder.Ascending);

        // Act
        context.OrderBy(order);

        // Assert
        context.OrderByClauses.Count.Should().Be(1);
    }

    [Fact]
    public void AddSameOrderByFails()
    {
        // Arrange
        var order = OrderBy.Create("name", TableTarget.Create("users", "u"), SortOrder.Ascending);

        // Act
        context.OrderBy(order);
        context.OrderBy(order);

        // Assert
        context.OrderByClauses.Count.Should().Be(1);
    }

    [Fact]
    public void WhereClauseSucceeds()
    {
        // Arrange
        var column = Column.Create("name", TableTarget.Create("users"));
        var clause = state.Conditionals.Column.Equals(column, 42);

        // Act
        context.Where(clause);

        // Assert
        context.WhereClause.Should().NotBeNull();
    }

    [Theory]
    [MemberData(nameof(JoinTests))]
    public void AddJoinSucceeds(Join join)
    {
        // Act
        context.Join(join);

        // Assert
        context.Joins.Count.Should().Be(1);
    }

    [Fact]
    public void AddJoinWithSameTargetsAndAliasFails()
    {
        // Arrange
        var from = TableTarget.Create("users", "u");
        var to = TableTarget.Create("accounts", "a");
        var join = Join.Create(from, to, JoinType.Default, null);

        // Act
        context.Join(join);
        context.Join(join);

        // Assert
        context.Joins.Count.Should().Be(1);
    }

    [Fact]
    public void DisposeClearsJoins()
    {
        // Arrange
        var from = TableTarget.Create("users", "u");
        var to = TableTarget.Create("accounts", "a");
        var join = Join.Create(from, to, JoinType.Default, null);

        context.Join(join);

        // Act
        context.Dispose();

        // Assert
        context.Joins.Count.Should().Be(0);
    }

    [Fact]
    public void DisposeClearsQueryTargets()
    {
        // Arrange
        var from = TableTarget.Create("users", "u");

        context.From(from);

        // Act
        context.Dispose();

        // Assert
        context.QueryTargets.Count.Should().Be(0);
    }

    [Fact]
    public void DisposeClearsWhereClause()
    {
        // Arrange
        var column = Column.Create("name", TableTarget.Create("users"));
        var clause = state.Conditionals.Column.Equals(column, 42);
        context.Where(clause);

        // Act
        context.Dispose();

        // Assert
        context.WhereClause.Should().BeNull();
    }

    [Fact]
    public void DisposeClearsOrderBy()
    {
        // Arrange
        var order = OrderBy.Create("name", TableTarget.Create("users"), SortOrder.Ascending);
        context.OrderBy(order);

        // Act
        context.Dispose();

        // Assert
        context.OrderByClauses.Count.Should().Be(0);
    }

    [Fact]
    public void DisposeClearsGroupBy()
    {
        // Arrange
        var group = GroupBy.Create("name", TableTarget.Create("users"));
        context.GroupBy(group);

        // Act
        context.Dispose();

        // Assert
        context.GroupByClauses.Count.Should().Be(0);
    }

    public static IEnumerable<object[]> ColumnTests
    {
        get
        {
            yield return new[] { Column.Create("name", "users", "u") };
            yield return new[] { Column.Create("name", TableTarget.Create("users", "u")) };
            yield return new[] { Column.Create("name", TableTarget.Create("users", "u"), "username") };
        }
    }

    public static IEnumerable<object[]> TableTests
    {
        get
        {
            yield return new[] { TableTarget.Create("users", "u") };
            yield return new[] { TableTarget.Create("users", "public") };
        }
    }

    public static IEnumerable<object[]> JoinTests
    {
        get
        {
            yield return new[] { Join.Create(null, null, JoinType.Default, null) };
        }
    }
}