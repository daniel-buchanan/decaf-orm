using System;
using System.Data;
using decaf.common.Logging;

namespace decaf.common.Connections;

public abstract class Connection : IConnection
{
    protected IDbConnection? DbConnection;
    protected readonly IConnectionDetails ConnectionDetails;

    /// <summary>
    /// Create an instance of a Connection.
    /// </summary>
    /// <param name="connectionDetails">The connection details to use.</param>
    protected Connection(IConnectionDetails connectionDetails)
    {
        ConnectionDetails = connectionDetails;
        State = ConnectionState.Unknown;
    }

    /// <inheritdoc/>
    public ConnectionState State { get; private set; }

    /// <inheritdoc/>
    public void Close()
    {
        if (DbConnection is null) return;
        if (DbConnection.State != System.Data.ConnectionState.Open &&
            DbConnection.State != System.Data.ConnectionState.Connecting &&
            DbConnection.State != System.Data.ConnectionState.Fetching &&
            DbConnection.State != System.Data.ConnectionState.Executing) return;
            
        DbConnection.Close();
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
        DbConnection?.Dispose();
    }

    /// <inheritdoc/>
    public void Open()
    {
        DbConnection ??= GetUnderlyingConnection();

        if (DbConnection.State != System.Data.ConnectionState.Closed &&
            DbConnection.State != System.Data.ConnectionState.Broken) return;
        
        DbConnection.Open();
        State = ConnectionState.Open;
    }

    /// <inheritdoc/>
    public string GetHash()
        => ConnectionDetails.GetHash();

    /// <inheritdoc/>
    public abstract IDbConnection GetUnderlyingConnection();

    /// <inheritdoc/>
    public TConnection GetUnderlyingConnectionAs<TConnection>()
        where TConnection : IDbConnection
        => (TConnection)GetUnderlyingConnection();
}