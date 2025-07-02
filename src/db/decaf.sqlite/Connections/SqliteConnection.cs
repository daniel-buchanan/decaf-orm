using System.Data;
using decaf.common.Connections;
using decaf.common.Exceptions;

namespace decaf.sqlite;

public class SqliteConnection(IConnectionDetails connectionDetails) 
    : Connection(connectionDetails), IConnection
{
    public override IDbConnection GetUnderlyingConnection()
    {
        var details = ConnectionDetails as ISqliteConnectionDetails;
        if (details is null) throw new MissingConnectionDetailsException("No ConnectionDetails found.");

        if (DbConnection != null)
            return DbConnection;
        
        DbConnection = new Microsoft.Data.Sqlite.SqliteConnection(details.GetConnectionString());
        return DbConnection;
    }
}