using System;
using System.Data;
using decaf.common.Connections;
using decaf.common.Logging;
using decaf.tests.common.Mocks;
using FluentAssertions;
using Xunit;

namespace decaf.core_tests.Connections
{
    public class ConnectionTests
    {
        private readonly IConnection connection;


        public ConnectionTests()
        {
            var logger = new DefaultLoggerProxy(new DecafOptions(), new StdOutputWrapper());
            var connectionDetails = new MockConnectionDetails();
            var dbOptions = new MockDatabaseOptions();
            this.connection = new MockConnection(dbOptions, logger, connectionDetails);
        }

        [Fact]
        public void GetHashSucceeds()
        {
            // Act
            var hash = this.connection.GetHash();

            // Assert
            hash.Should().NotBeNull();
        }

        [Fact]
        public void GetUnderlyingConnectionSucceeds()
        {
            // Act
            var conn = this.connection.GetUnderlyingConnection();

            // Assert
            conn.Should().NotBeNull();
        }

        [Fact]
        public void GetUnderlyingConnectionAsSucceeds()
        {
            // Act
            var conn = this.connection.GetUnderlyingConnectionAs<IDbConnection>();

            // Assert
            conn.Should().NotBeNull();
            conn.Should().BeAssignableTo<IDbConnection>();
        }

        [Fact]
        public void OpenResultsInStateOpen()
        {
            // Act
            this.connection.Open();

            // Assert
            this.connection.State.Should().Be(common.Connections.ConnectionState.Open);
        }

        [Fact]
        public void CloseResultsInStateClosed ()
        {
            // Act
            this.connection.Open();
            this.connection.Close();

            // Assert
            this.connection.State.Should().Be(common.Connections.ConnectionState.Closed);
        }

        [Fact]
        public void DefaultConnectionStateIsUnknown()
        {
            // Assert
            this.connection.State.Should().Be(common.Connections.ConnectionState.Unknown);
        }

        [Fact]
        public void DisposeSucceeds()
        {
            // Act
            Action method = () => this.connection.Dispose();

            // Assert
            method.Should().NotThrow<ObjectDisposedException>();
        }
    }
}

