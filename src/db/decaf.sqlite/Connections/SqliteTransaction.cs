using System.Data;
using decaf.common.Connections;
using decaf.common.Exceptions;
using decaf.common.Logging;
using ConnectionState = System.Data.ConnectionState;

namespace decaf.sqlite;

public class SqliteTransaction : Transaction
{
    private IDbTransaction? currentTransaction = null;
    
    public SqliteTransaction(
        Guid id,
        ILoggerProxy logger,
        IConnection connection,
        DecafOptions options) :
        base(id, logger, connection, options)
    { }

    public override IDbTransaction GetUnderlyingTransaction()
    {
        var sqliteConnection = this.connection as SqliteConnection;
        if (sqliteConnection == null)
            throw new InvalidConnectionException($"The provided connection for Transaction {this.Id} is not of the type \"SQLiteConnection\"");

        if (currentTransaction is not null)
            return currentTransaction;
        
        var conn = sqliteConnection.GetUnderlyingConnection();
        
        if(conn.State != ConnectionState.Open)
            conn.Open();

        currentTransaction = conn.BeginTransaction();
        
        return currentTransaction;
    }
}