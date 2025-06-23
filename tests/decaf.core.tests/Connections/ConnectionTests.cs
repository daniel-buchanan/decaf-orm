using System;
using System.Data;
using decaf.common.Logging;
using decaf.tests.common.Mocks;
using FluentAssertions;
using Xunit;

namespace decaf.core_tests.Connections;

public class ConnectionTests
{
    private readonly MockConnection connection;


    public ConnectionTests()
    {
        var logger = new DefaultLoggerProxy(new DecafOptions(), new StdOutputWrapper());
        var connectionDetails = new MockConnectionDetails();
        var dbOptions = new MockDatabaseOptions();
        connection = new MockConnection(dbOptions, logger, connectionDetails);
    }

    [Fact]
    public void GetHashSucceeds()
    {
        // Act
        var hash = connection.GetHash();

        // Assert
        hash.Should().NotBeNull();
    }

    [Fact]
    public void GetUnderlyingConnectionSucceeds()
    {
        // Act
        var conn = connection.GetUnderlyingConnection();

        // Assert
        conn.Should().NotBeNull();
    }

    [Fact]
    public void GetUnderlyingConnectionAsSucceeds()
    {
        // Act
        var conn = connection.GetUnderlyingConnectionAs<IDbConnection>();

        // Assert
        conn.Should().NotBeNull();
        conn.Should().BeAssignableTo<IDbConnection>();
    }

    [Fact]
    public void OpenResultsInStateOpen()
    {
        // Act
        connection.Open();

        // Assert
        connection.State.Should().Be(common.Connections.ConnectionState.Open);
    }

    [Fact]
    public void CloseResultsInStateClosed ()
    {
        // Act
        connection.Open();
        connection.Close();

        // Assert
        connection.State.Should().Be(common.Connections.ConnectionState.Closed);
    }

    [Fact]
    public void DefaultConnectionStateIsUnknown()
    {
        // Assert
        connection.State.Should().Be(common.Connections.ConnectionState.Unknown);
    }

    [Fact]
    public void DisposeSucceeds()
    {
        // Act
        Action method = () => connection.Dispose();

        // Assert
        method.Should().NotThrow<ObjectDisposedException>();
    }
}