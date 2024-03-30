using System;
using decaf.common;
using decaf.common.Connections;
using decaf.tests.common.Mocks;
using decaf.tests.common.Models;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace decaf.core_tests;

public class UnitOfWorkTests : CoreTestBase
{
    [Fact]
    public void UnitOfWorkFluentSuccess()
    {
        // Arrange
        var decaf = provider.GetService<IDecaf>();
        var unit = decaf.BuildUnit();
        var hasException = false;
        User result = null;

        // Act
        unit.OnException(_ => hasException = true)
            .OnSuccess(() => hasException = false)
            .Query(q =>
            {
                result = q.Select()
                    .From<User>(u => u)
                    .Where(u => u.Id == 1)
                    .SelectAll<User>(u => u)
                    .SingleOrDefault();
            })
            .PersistChanges();

        // Assert
        hasException.Should().BeFalse();
    }

    [Fact]
    public void CreateUnitSucceeds()
    {
        // Arrange
        var factory = provider.GetService<IUnitOfWorkFactory>();

        // Act
        Action method = () => factory.Create();

        // Assert
        method.Should().NotThrow();
    }
    
    [Fact]
    public void CreateUnitWithDetailsSucceeds()
    {
        // Arrange
        var factory = provider.GetService<IUnitOfWorkFactory>();

        // Act
        Action method = () => factory.Create(new MockConnectionDetails());

        // Assert
        method.Should().NotThrow();
    }
}