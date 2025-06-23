using System;
using System.Collections.Generic;
using decaf.common;
using decaf.common.Utilities;
using decaf.state;
using decaf.state.QueryTargets;
using decaf.state.ValueSources.Insert;
using FluentAssertions;
using Xunit;

namespace decaf.core_tests;

public class InsertQueryContextTests
{
    private readonly IInsertQueryContext context;

    public InsertQueryContextTests()
    {
        var aliasManager = AliasManager.Create();
        var hashProvider = new HashProvider();
        context = InsertQueryContext.Create(aliasManager, hashProvider);
    }

    [Fact]
    public void QueryTypeIsInsert()
    {
        // Act
        var queryType = context.Kind;

        // Assert
        queryType.Should().Be(QueryTypes.Insert);
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
        context.Column(column);

        // Assert
        context.Columns.Count.Should().Be(1);
    }

    [Theory]
    [MemberData(nameof(ColumnTests))]
    public void AddExistingColumnDoesNothing(Column column)
    {
        // Arrange
        context.Column(column);

        // Act
        context.Column(column);

        // Assert
        context.Columns.Count.Should().Be(1);
    }

    [Theory]
    [MemberData(nameof(TableTests))]
    public void InsertIntoSucceeds(ITableTarget target)
    {
        // Act
        context.Into(target);

        // Assert
        context.QueryTargets.Count.Should().Be(1);
    }

    [Theory]
    [MemberData(nameof(TableTests))]
    public void InsertIntoSameDoesNothing(ITableTarget target)
    {
        // Arrange
        context.Into(target);

        // Act
        context.Into(target);

        // Assert
        context.Target.Should().Be(target);
        context.QueryTargets.Count.Should().Be(1);
    }

    [Fact]
    public void AddValuesSucceeds()
    {
        // Arrange
        var values = new object[] { "hello", "world", 42 };

        // Act
        context.Value(values);

        // Assert
        context.Source.Should().BeAssignableTo<IInsertStaticValuesSource>();
    }

    [Fact]
    public void AddValuesAfterSettingSourceToQueryDoesNothing()
    {
        // Arrange
        context.From(QueryValuesSource.Create(null));
        var values = new object[] { "hello", "world", 42 };

        // Act
        context.Value(values);

        // Assert
        context.Source.Should().BeAssignableTo<QueryValuesSource>();
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
}