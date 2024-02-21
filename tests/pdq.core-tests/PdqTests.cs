using Xunit;
using pdq.common;
using pdq.tests.common.Mocks;
using FluentAssertions;
using pdq.common.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace pdq.core_tests
{
    public class PdqTests
    {
        static IPdq GetPdq(bool needsConnectionDetails = false)
        {
            var services = new ServiceCollection();
            services.AddPdq(b =>
            {
                b.InjectUnitOfWorkAsScoped();
                b.UseMockDatabase();
                if (needsConnectionDetails) b.WithMockConnectionDetails();
            });
            var provider = services.BuildServiceProvider();
            return provider.GetRequiredService<IPdq>();
        }

        [Fact]
        public void Begin_MissingConnectionDetailsThrows()
        {
            // Arrange
            var pdq = GetPdq();

            // Act
            var method = () => pdq.Begin();

            // Assert
            method.Should().Throw<MissingConnectionDetailsException>();
        }

        [Fact]
        public void BeginQuery_MissingConnectionDetailsThrows()
        {
            // Arrange
            var pdq = GetPdq();

            // Act
            var method = () => pdq.BeginQuery();

            // Assert
            method.Should().Throw<MissingConnectionDetailsException>();
        }

        [Fact]
        public void BeginAsync_MissingConnectionDetailsThrows()
        {
            // Arrange
            var pdq = GetPdq();

            // Act
            var method = () => pdq.BeginAsync();

            // Assert
            method.Should().ThrowAsync<MissingConnectionDetailsException>();
        }

        [Fact]
        public void BeginQueryAsync_MissingConnectionDetailsThrows()
        {
            // Arrange
            var pdq = GetPdq();

            // Act
            var method = () => pdq.BeginQueryAsync();

            // Assert
            method.Should().ThrowAsync<MissingConnectionDetailsException>();
        }

        [Fact]
        public void Begin_ProvidedConnectionDetailsDoesNotThrow()
        {
            // Arrange
            var pdq = GetPdq();
            var connectionDetails = new MockConnectionDetails();

            // Act
            var method = () => pdq.Begin(connectionDetails);

            // Assert
            method.Should().NotThrow<MissingConnectionDetailsException>();
        }

        [Fact]
        public void BeginQuery_ProvidedConnectionDetailsDoesNotThrow()
        {
            // Arrange
            var pdq = GetPdq();
            var connectionDetails = new MockConnectionDetails();

            // Act
            var method = () => pdq.BeginQuery(connectionDetails);

            // Assert
            method.Should().NotThrow<MissingConnectionDetailsException>();
        }

        [Fact]
        public void BeginAsync_ProvidedConnectionDetailsDoesNotThrow()
        {
            // Arrange
            var pdq = GetPdq();
            var connectionDetails = new MockConnectionDetails();

            // Act
            var method = () => pdq.BeginAsync(connectionDetails);

            // Assert
            method.Should().NotThrowAsync<MissingConnectionDetailsException>();
        }

        [Fact]
        public void BeginQueryAsync_ProvidedConnectionDetailsDoesNotThrow()
        {
            // Arrange
            var pdq = GetPdq();
            var connectionDetails = new MockConnectionDetails();

            // Act
            var method = () => pdq.BeginQueryAsync(connectionDetails);

            // Assert
            method.Should().NotThrowAsync<MissingConnectionDetailsException>();
        }
    }
}