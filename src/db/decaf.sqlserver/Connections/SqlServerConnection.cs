using System.Data;
using decaf.common.Connections;
using decaf.common.Logging;

namespace decaf.sqlserver;

public class SqlServerConnection : Connection, IConnection
{
    public SqlServerConnection(
        ILoggerProxy logger,
        IConnectionDetails connectionDetails)
        : base(logger, connectionDetails)
    {
    }

    public override IDbConnection GetUnderlyingConnection()
    {
        var details = connectionDetails as ISqlServerConnectionDetails;
        return new Microsoft.Data.SqlClient.SqlConnection(details.GetConnectionString());
    }
}