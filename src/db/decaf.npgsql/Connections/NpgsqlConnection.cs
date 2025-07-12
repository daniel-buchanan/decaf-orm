using System.Data;
using decaf.common.Connections;
using decaf.common.Exceptions;
using decaf.common.Logging;

namespace decaf.npgsql;

public class NpgsqlConnection(IConnectionDetails connectionDetails) 
    : Connection(connectionDetails), IConnection
{
    public override IDbConnection GetUnderlyingConnection()
    {
        var details = ConnectionDetails as INpgsqlConnectionDetails;
        if(details is null) throw new MissingConnectionDetailsException("Missing connection details");
        return new Npgsql.NpgsqlConnection(details.GetConnectionString());
    }
}