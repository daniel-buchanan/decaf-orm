using System;
using System.Linq;
using decaf.common;
using decaf.common.Connections;
using decaf.tests.common.Mocks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace decaf.core_tests;

public class DecafOrmServiceCollectionTests
{
    [Fact]
    public void CreateSuccessfully()
    {
        // Arrange
        var services = new ServiceCollection();
        
        // Act
        Action method = () => DecafOrmServiceCollection.Create(services);

        // Assert
        method.Should().NotThrow();
    }

    [Fact]
    public void WithConnectionSingleton()
    {
        // Arrange
        var services = DecafOrmServiceCollection.Create(new ServiceCollection());
        var connectionDetails = new MockConnectionDetails();

        // Act
        services.WithConnection(connectionDetails);

        // Assert
        services.Should().HaveCount(1);
        var sd = services.First(s => s.ServiceType == typeof(IConnectionDetails));
        sd.ServiceType.Should().Be(typeof(IConnectionDetails));
        sd.ImplementationType.Should().BeNull();
        sd.ImplementationInstance.Should().Be(connectionDetails);
        sd.Lifetime.Should().Be(ServiceLifetime.Singleton);
    }

    [Theory]
    [InlineData(ServiceLifetime.Scoped)]
    [InlineData(ServiceLifetime.Singleton)]
    [InlineData(ServiceLifetime.Transient)]
    public void WithConnectionBuilder(ServiceLifetime lifetime)
    {
        // Arrange
        var services = DecafOrmServiceCollection.Create(new ServiceCollection());

        // Act
        services.WithConnection<MockConnectionDetails>(c =>
        {
            c.Hostname = "localhost";
        }, lifetime);

        // Assert
        services.Should().HaveCount(1);
        var sd = services.First(s => s.ServiceType == typeof(IConnectionDetails));
        sd.ImplementationType.Should().BeNull();
        sd.ServiceType.Should().Be(typeof(IConnectionDetails));
        sd.ImplementationFactory.Should().NotBeNull();
        sd.Lifetime.Should().Be(lifetime);
    }
}