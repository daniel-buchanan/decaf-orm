using System.Threading;
using System.Threading.Tasks;
using decaf.common.Connections;
using decaf.common.Logging;

namespace decaf.npgsql;

public class NpgsqlConnectionFactory(ILoggerProxy logger) 
    : ConnectionFactory(logger)
{
    protected override Task<IConnection> ConstructConnectionAsync(IConnectionDetails connectionDetails, CancellationToken cancellationToken = default)
    {
        IConnection conn = new NpgsqlConnection(connectionDetails);
        return Task.FromResult(conn);
    }
}