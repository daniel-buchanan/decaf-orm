using System;
using decaf.common.Connections;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace decaf.core_tests.Connections;

public class TransactionTests : CoreTestBase
{
    [Fact]
    public void TransactionDisposeSuccessful()
    {
        // Arrange
        var factory = provider.GetService<ITransactionFactory>();
        var connDetails = provider.GetService<IConnectionDetails>();
        var t = factory.Get(connDetails);

        // Act
        Action method = () => t.Dispose();

        // Assert
        method.Should().NotThrow();
    }
}