using System;
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
        private readonly IUnitOfWork uow;

        public TransientTests()
        {
            var services = new ServiceCollection();
            services.AddPdq(o =>
            {
                o.UseMockDatabase();
            });
            services.AddScoped<IConnectionDetails, MockConnectionDetails>();
            var provider = services.BuildServiceProvider();
            this.uow = provider.GetService<IUnitOfWork>();
        }

        [Fact]
        public void CanGetTransientSucceeds()
        {
            // Act
            Action method = () => this.uow.Begin();

            // Assert
            method.Should().NotThrow();
        }

        [Fact]
        public void NewTransientHasId()
        {
            // Act
            var transient = this.uow.Begin();

            // Assert
            transient.Should().NotBeNull();
            transient.Id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void CanGetConnectionSucceeds()
        {
            // Arrange
            var transient = this.uow.Begin() as ITransientInternal;

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
            var transient = this.uow.Begin() as ITransientInternal;

            // Act
            Func<ITransaction> method = () => transient.Transaction;

            // Assert
            method.Should().NotThrow();
            var result = method();
            result.Should().NotBeNull();
        }
    }
}

