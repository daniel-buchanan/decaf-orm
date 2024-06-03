using System;
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
            .Named("test")
            .WithColumns(c => c.Named("Id").AsType<int>())
            .WithPrimaryKey(c => c.Named("Id"));
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
            .From("test", "t")
            .Select(b => new
            {
                Id = b.Is<int>("Id", "t")
            })
            .Dynamic()
            .ToList();
        
        // Assert
        items.Should().BeEmpty();
    }

    [Fact]
    public void InsertedItemReturned()
    {
        // Arrange
        var decaf = provider.GetService<IDecaf>();
        var q = decaf.Query().Insert().Into("test", "t").Columns(b => new
            {
                Id = b.Is<int>()
            })
            .Value(new
            {
                Id = 1
            });
        q.Execute();

        // Act
        var result = decaf.Query().Select().From("test", "t")
            .Where(b => b.Column("Id", "t").Is().EqualTo(1))
            .SelectDynamic(b => new { Id = b.Is<int>("Id", "t") })
            .FirstOrDefault();

        // Assert
        Assert.Equal(1, result.Id);
    }
}