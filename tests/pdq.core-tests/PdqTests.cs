using Xunit;
using Moq;
using pdq.common;
using pdq.common.Logging;
using pdq.common.Connections;
using pdq.tests.common.Mocks;
using pdq.common.Utilities;
using FluentAssertions;
using pdq.common.Exceptions;

namespace pdq.core_tests
{
    public class PdqTests
    {
        static IPdq GetPdq()
        {
            var logger = Mock.Of<ILoggerProxy>();
            var options = new PdqOptions();
            var connectionFactory = new MockConnectionFactory(logger);
            var transactionFactory = new MockTransactionFactory(connectionFactory, logger, options);
            var sqlFactory = new MockSqlFactory();
            var hashProvider = new HashProvider();
            var transientFactory = new UnitOfWorkFactory(options, logger, transactionFactory, sqlFactory, hashProvider);
            return new Pdq(logger, transientFactory);
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