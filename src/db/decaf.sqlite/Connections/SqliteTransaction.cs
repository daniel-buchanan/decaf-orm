using System.Data;
using decaf.common.Connections;
using decaf.common.Exceptions;
using decaf.common.Logging;
using ConnectionState = System.Data.ConnectionState;

namespace decaf.sqlite;

public class SqliteTransaction(
    Guid id,
    ILoggerProxy logger,
    IConnection connection,
    DecafOptions options)
    : Transaction(id, logger, connection, options)
{
    private IDbTransaction? currentTransaction;

    public override IDbTransaction GetUnderlyingTransaction()
    {
        var sqliteConnection = Connection as SqliteConnection;
        if (sqliteConnection == null)
            throw new InvalidConnectionException($"The provided connection for Transaction {Id} is not of the type \"SQLiteConnection\"");

        if (currentTransaction is not null)
            return currentTransaction;
        
        var conn = sqliteConnection.GetUnderlyingConnection();
        
        if(conn.State != ConnectionState.Open)
            conn.Open();

        currentTransaction = conn.BeginTransaction();
        
        return currentTransaction;
    }
}