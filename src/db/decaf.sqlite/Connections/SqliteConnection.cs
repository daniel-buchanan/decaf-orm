using System.Data;
using decaf.common.Connections;
using decaf.common.Exceptions;
using decaf.common.Logging;

namespace decaf.sqlite;

public class SqliteConnection : Connection, IConnection
{
    /// <inheritdoc />
    public SqliteConnection(
        ILoggerProxy logger, 
        IConnectionDetails connectionDetails) : 
        base(logger, connectionDetails) { }

    public override IDbConnection GetUnderlyingConnection()
    {
        var details = connectionDetails as ISqliteConnectionDetails;
        if (details is null) throw new MissingConnectionDetailsException("No ISqliteConnectionDetails found.");

        if (dbConnection != null)
            return dbConnection;
        
        dbConnection = new Microsoft.Data.Sqlite.SqliteConnection(details.GetConnectionString());
        return dbConnection;
    }
}