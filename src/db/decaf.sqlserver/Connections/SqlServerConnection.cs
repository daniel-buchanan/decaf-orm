using System.Data;
using decaf.common.Connections;
using decaf.common.Exceptions;
using decaf.common.Logging;

namespace decaf.sqlserver;

public class SqlServerConnection(IConnectionDetails connectionDetails) 
    : Connection(connectionDetails), IConnection
{
    public override IDbConnection GetUnderlyingConnection()
    {
        var details = ConnectionDetails as ISqlServerConnectionDetails;
        if (details is null) throw new MissingConnectionDetailsException("No ConnectionDetails provided");
        return new Microsoft.Data.SqlClient.SqlConnection(details.GetConnectionString());
    }
}