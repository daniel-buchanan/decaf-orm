using System.Threading;
using System.Threading.Tasks;
using decaf.common.Connections;
using decaf.common.Logging;

namespace decaf.sqlserver;

public class SqlServerConnectionFactory : ConnectionFactory
{
    public SqlServerConnectionFactory(ILoggerProxy logger)
        : base(logger)
    {
    }

    protected override Task<IConnection> ConstructConnectionAsync(IConnectionDetails connectionDetails, CancellationToken cancellationToken = default)
    {
        var conn = new SqlServerConnection(logger, connectionDetails);
        return Task.FromResult(conn as IConnection);
    }
}