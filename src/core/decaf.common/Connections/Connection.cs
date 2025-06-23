using System;
using System.Data;
using decaf.common.Logging;

namespace decaf.common.Connections;

public abstract class Connection : IConnection
{
    protected IDbConnection dbConnection;
    protected readonly ILoggerProxy logger;
    protected IConnectionDetails connectionDetails;

    /// <summary>
    /// Create an instance of a Connection.
    /// </summary>
    /// <param name="logger">The logger to use to log any details.</param>
    /// <param name="connectionDetails">The connection details to use.</param>
    protected Connection(
        ILoggerProxy logger,
        IConnectionDetails connectionDetails)
    {
        this.logger = logger;
        this.connectionDetails = connectionDetails;
        State = ConnectionState.Unknown;
    }

    /// <inheritdoc/>
    public ConnectionState State { get; private set; }

    /// <inheritdoc/>
    public void Close()
    {
        if (dbConnection == null) return;
        if (dbConnection.State != System.Data.ConnectionState.Open &&
            dbConnection.State != System.Data.ConnectionState.Connecting &&
            dbConnection.State != System.Data.ConnectionState.Fetching &&
            dbConnection.State != System.Data.ConnectionState.Executing) return;
            
        dbConnection.Close();
        State = ConnectionState.Closed;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;
        dbConnection.Dispose();
    }

    /// <inheritdoc/>
    public void Open()
    {
        if (dbConnection == null)
            dbConnection = GetUnderlyingConnection();

        if (dbConnection.State == System.Data.ConnectionState.Closed ||
            dbConnection.State == System.Data.ConnectionState.Broken)
        {
            dbConnection.Open();
            State = ConnectionState.Open;
        }
    }

    /// <inheritdoc/>
    public string GetHash()
        => connectionDetails.GetHash();

    /// <inheritdoc/>
    public abstract IDbConnection GetUnderlyingConnection();

    /// <inheritdoc/>
    public TConnection GetUnderlyingConnectionAs<TConnection>()
        where TConnection : IDbConnection
        => (TConnection)GetUnderlyingConnection();
}