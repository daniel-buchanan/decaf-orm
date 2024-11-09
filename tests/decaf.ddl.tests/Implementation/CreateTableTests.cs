using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using decaf.common;
using decaf.common.Connections;
using decaf.common.Exceptions;
using decaf.common.Utilities.Reflection;
using decaf.state;
using decaf.state.Ddl.Definitions;
using decaf.tests.common.Mocks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace decaf.ddl.Implementation;

public class CreateTableTests
{
    const string tableName = "bob";
    readonly IQueryContainer query;

    public CreateTableTests()
    {
        var services = new ServiceCollection();
        services.AddDecaf(d => d.UseMockDatabase())
            .WithConnection<IConnectionDetails, MockConnectionDetails>();
        var provider = services.BuildServiceProvider();
        var decaf = provider.GetRequiredService<IDecaf>();
        var unit = decaf.BuildUnit();
        query = unit.GetQuery();
    }
    
    [Fact]
    public void WithNameSucceeds()
    {
        // Arrange
        var impl = query.CreateTable();
        
        // Act
        impl.Named(tableName);

        // Assert
        var o = query.Context as ICreateTableQueryContext;
        o!.Name.Should().Be(tableName);
    }

    [Theory]
    [MemberData(nameof(ColumnTests))]
    public void WithColumnsSucceeds(
        Expression<Action<IDdlColumnBuilder>> columnBuilder, 
        IColumnDefinition expected)
    {
        // Arrange
        var impl = query.CreateTable();

        // Act
        impl.Named(tableName)
            .WithColumns(columnBuilder);

        // Assert
        var c = query.Context as ICreateTableQueryContext;
        c!.Columns.Should().ContainEquivalentOf(expected);
    }

    [Theory]
    [MemberData(nameof(IndexTests))]
    public void WithIndexSucceeds(Expression<Action<IDdlColumnBuilder>>[] columns, IIndexDefinition index)
    {
        // Arrange
        var impl = query.CreateTable();

        // Act
        impl.Named(tableName)
            .WithIndex(columns);

        // Assert
        var c = query.Context as ICreateTableQueryContext;
        c!.Indexes.Should().ContainEquivalentOf(index);
    }

    [Fact]
    public void WithIndexThrowsWhenNoTable()
    {
        // Arrange
        var impl = query.CreateTable();

        // Act
        Action method = () => impl.WithIndex(c => c.Named("id"));

        // Assert
        method.Should().Throw<PropertyNotProvidedException>();
    }

    [Fact]
    public void WithIndexNameSucceeds()
    {
        // Arrange
        const string indexName = "pk";
        var impl = query.CreateTable();

        // Act
        impl.Named(tableName)
            .WithIndex(indexName, c => c.Named("id"));

        // Assert
        var c = query.Context as ICreateTableQueryContext;
        c!.Indexes.Should()
            .ContainEquivalentOf(
                IndexDefinition.Create(
                    indexName, 
                    tableName, 
                    ColumnDefinition.Create("id")));
    }

    public static IEnumerable<object[]> ColumnTests
    {
        get
        {
            yield return GetColumn<int>("id");
            yield return GetColumn<int>("id", false);
            yield return GetColumn<string>("this_is_a_name");
            yield return GetColumn<string>("this_is_a_name", false);
            yield return GetColumn<DateTime>("timestamp");
            yield return GetColumn<DateTime>("timestamp", false);
        }
    }

    public static IEnumerable<object[]> IndexTests
    {
        get
        {
            yield return GetIndex(tableName, [c => c.Named("id")]);
            yield return GetIndex(tableName, [c => c.Named("id"), c => c.Named("sub")]);
        }
    }

    private static object[] GetColumn<T>(string name, bool nullable = true)
        => GetColumn(name, typeof(T), nullable);

    private static object[] GetColumn(string name, Type type, bool nullable = true) =>
    [
        GetExpression(b => b.Named(name).AsType(type).IsNullable(nullable)),
        ColumnDefinition.Create(name, type, nullable)
    ];
    
    private static object[] GetIndex(string table, Expression<Action<IDdlColumnBuilder>>[] columns)
    {
        var id = IndexDefinition.Create(table, columns.Select(a =>
        {
            var c = new DdlColumnBuilder(new ExpressionHelper(new ReflectionHelper()));
            var f = a.Compile();
            f(c);
            return c.Build();
        }).ToArray());

        return [columns, id];
    }

    private static Expression<Action<IDdlColumnBuilder>> GetExpression(Expression<Action<IDdlColumnBuilder>> input)
        => input;
}