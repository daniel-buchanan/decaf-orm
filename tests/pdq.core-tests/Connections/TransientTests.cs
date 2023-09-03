using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using pdq.common;
using pdq.common.Connections;
using pdq.tests.common.Mocks;
using Xunit;

namespace pdq.core_tests.Connections
{
    public class TransientTests
    {
        private readonly IPdq pdq;

        public TransientTests()
        {
            var services = new ServiceCollection();
            services.AddPdq(o =>
            {
                o.UseMockDatabase();
            });
            services.AddScoped<IConnectionDetails, MockConnectionDetails>();
            var provider = services.BuildServiceProvider();
            this.pdq = provider.GetService<IPdq>();
        }

        [Fact]
        public void CanGetTransientSucceeds()
        {
            // Act
            Action method = () => this.pdq.Begin();

            // Assert
            method.Should().NotThrow();
        }

        [Fact]
        public void NewTransientHasId()
        {
            // Act
            var transient = this.pdq.Begin();

            // Assert
            transient.Should().NotBeNull();
            transient.Id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void CanGetConnectionSucceeds()
        {
            // Arrange
            var transient = this.pdq.Begin() as ITransientInternal;

            // Act
            Func<IConnection> method = () => transient.Connection;

            // Assert
            method.Should().NotThrow();
            var result = method();
            result.Should().NotBeNull();
        }

        [Fact]
        public void CanGetTransactionSucceeds()
        {
            // Arrange
            var transient = this.pdq.Begin() as ITransientInternal;

            // Act
            Func<ITransaction> method = () => transient.Transaction;

            // Assert
            method.Should().NotThrow();
            var result = method();
            result.Should().NotBeNull();
        }

        [Fact]
        public void NotifyQueryDisposed_UnknownQueryDoesNothing()
        {
            // Arrange
            var transient = this.pdq.Begin() as ITransientInternal;

            // Act
            transient.NotifyQueryDisposed(Guid.Empty);
            var method = () => transient.Query();

            // Assert
            method.Should().NotThrow<ObjectDisposedException>();
        }
    }
}

