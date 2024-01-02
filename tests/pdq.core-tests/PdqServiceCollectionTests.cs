using System;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using pdq.common;
using pdq.tests.common.Mocks;
using Xunit;

namespace pdq.core_tests;

public class PdqServiceCollectionTests
{
    public PdqServiceCollectionTests()
    {
        
    }

    [Fact]
    public void CreateSuccessfully()
    {
        // Arrange
        var services = new ServiceCollection();
        
        // Act
        Action method = () => PdqServiceCollection.Create(services);

        // Assert
        method.Should().NotThrow();
    }

    [Fact]
    public void WithConnectionSingleton()
    {
        // Arrange
        var services = PdqServiceCollection.Create(new ServiceCollection());
        var connectionDetails = new MockConnectionDetails();

        // Act
        services.WithConnection(connectionDetails);

        // Assert
        services.Should().HaveCount(1);
        var sd = services.First(s => s.ServiceType == typeof(MockConnectionDetails));
        sd.ServiceType.Should().Be(typeof(MockConnectionDetails));
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
        var services = PdqServiceCollection.Create(new ServiceCollection());

        // Act
        services.WithConnection<MockConnectionDetails>(c =>
        {
            c.Hostname = "localhost";
        }, lifetime);

        // Assert
        services.Should().HaveCount(1);
        var sd = services.First(s => s.ServiceType == typeof(MockConnectionDetails));
        sd.ServiceType.Should().Be(typeof(MockConnectionDetails));
        sd.ImplementationType.Should().BeNull();
        sd.ImplementationFactory.Should().NotBeNull();
        sd.Lifetime.Should().Be(lifetime);
    }
}