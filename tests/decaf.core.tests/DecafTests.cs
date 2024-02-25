using decaf.common;
using decaf.common.Exceptions;
using decaf.tests.common.Mocks;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace decaf.core_tests
{
    public class DecafTests
    {
        static IDecaf GetDecaf(bool needsConnectionDetails = false)
        {
            var services = new ServiceCollection();
            services.AddDecafOrm(b =>
            {
                b.InjectUnitOfWorkAsScoped();
                b.UseMockDatabase();
                if (needsConnectionDetails) b.WithMockConnectionDetails();
            });
            var provider = services.BuildServiceProvider();
            return provider.GetRequiredService<IDecaf>();
        }

        [Fact]
        public void Begin_MissingConnectionDetailsThrows()
        {
            // Arrange
            var pdq = GetDecaf();

            // Act
            var method = () => pdq.Begin();

            // Assert
            method.Should().Throw<MissingConnectionDetailsException>();
        }

        [Fact]
        public void BeginQuery_MissingConnectionDetailsThrows()
        {
            // Arrange
            var pdq = GetDecaf();

            // Act
            var method = () => pdq.BeginQuery();

            // Assert
            method.Should().Throw<MissingConnectionDetailsException>();
        }

        [Fact]
        public void BeginAsync_MissingConnectionDetailsThrows()
        {
            // Arrange
            var pdq = GetDecaf();

            // Act
            var method = () => pdq.BeginAsync();

            // Assert
            method.Should().ThrowAsync<MissingConnectionDetailsException>();
        }

        [Fact]
        public void BeginQueryAsync_MissingConnectionDetailsThrows()
        {
            // Arrange
            var pdq = GetDecaf();

            // Act
            var method = () => pdq.BeginQueryAsync();

            // Assert
            method.Should().ThrowAsync<MissingConnectionDetailsException>();
        }

        [Fact]
        public void Begin_ProvidedConnectionDetailsDoesNotThrow()
        {
            // Arrange
            var pdq = GetDecaf();
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
            var pdq = GetDecaf();
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
            var pdq = GetDecaf();
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
            var pdq = GetDecaf();
            var connectionDetails = new MockConnectionDetails();

            // Act
            var method = () => pdq.BeginQueryAsync(connectionDetails);

            // Assert
            method.Should().NotThrowAsync<MissingConnectionDetailsException>();
        }
    }
}