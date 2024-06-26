using System;
using System.Collections.Generic;
using decaf.common;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace decaf.sqlite.tests;

public class LiveTests
{
    private readonly IServiceProvider provider;
    
    public LiveTests()
    {
        var services = new ServiceCollection();
        services.AddDecaf(o =>
        {
            o.OverrideDefaultLogLevel(LogLevel.Debug);
            o.TrackUnitsOfWork();
            o.CloseConnectionsOnCommitOrRollback();
            o.UseSqlite(oo =>
            {
                oo.InMemory();
            });
        });

        provider = services.BuildServiceProvider();
        var decaf = provider.GetService<IDecaf>();
        var d = decaf.Query()
            .CreateTable()
            .Named(nameof(TempTbl))
            .WithColumns(
                c => c.Named(nameof(TempTbl.Id)).AsType<int>(),
                c => c.Named(nameof(TempTbl.Name)).AsType<string>())
            .WithPrimaryKey(c => c.Named(nameof(TempTbl.Id)));
        var sql = d.GetSql();
        d.Execute();
    }

    [Fact]
    public void NoItemsReturnsEmpty()
    {
        // Arrange
        var decaf = provider.GetService<IDecaf>();
        
        // Act
        var items = decaf.Query()
            .Select()
            .From<TempTbl>(t=> t)
            .SelectAll<TempTbl>(t => t)
            .ToList();
        
        // Assert
        items.Should().BeEmpty();
    }

    [Fact]
    public void InsertedItemReturned()
    {
        // Arrange
        var decaf = provider.GetService<IDecaf>();
        decaf.Query()
            .Insert()
            .Into<TempTbl>()
            .Columns(b => new
            {
                Id = b.Id,
                Name = b.Name
            })
            .Value(new TempTbl() { Id = 1, Name = "Bob" })
            .Execute();

        // Act
        var result = decaf.Query()
            .Select()
            .From<TempTbl>(t=> t)
            .SelectAll<TempTbl>(t => t)
            .FirstOrDefault();

        // Assert
        Assert.Equal(1, result.Id);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(1000)]
    public void InsertedNRowsReturned(int n)
    {
        // Arrange
        var items = GenerateItemsToInsert(n);
        var decaf = provider.GetService<IDecaf>();
        decaf.Query()
            .Insert()
            .Into<TempTbl>()
            .Columns(b => new
            {
                Id = b.Id,
                Name = b.Name
            })
            .Values(items)
            .Execute();

        // Act
        var result = decaf.Query()
            .Select()
            .From<TempTbl>(t=> t)
            .SelectAll<TempTbl>(t => t)
            .ToList();

        // Assert
        result.Should().HaveCount(n);
    }

    private static IEnumerable<TempTbl> GenerateItemsToInsert(int n)
    {
        for(var i = 0; i < n; i++)
        {
            yield return new TempTbl()
            {
                Id = i + 1,
                Name = $"Name={i}"
            };
        }
    }
}

class TempTbl
{
    public int Id { get; set; }
    public string Name { get; set; }
}