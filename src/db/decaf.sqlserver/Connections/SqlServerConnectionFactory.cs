using System.Threading;
using System.Threading.Tasks;
using decaf.common.Connections;
using decaf.common.Logging;

namespace decaf.sqlserver;

public class SqlServerConnectionFactory(ILoggerProxy logger) 
    : ConnectionFactory(logger)
{
    protected override Task<IConnection> ConstructConnectionAsync(
        IConnectionDetails connectionDetails, 
        CancellationToken cancellationToken = default)
    {
        IConnection conn = new SqlServerConnection(connectionDetails);
        return Task.FromResult(conn);
    }
}