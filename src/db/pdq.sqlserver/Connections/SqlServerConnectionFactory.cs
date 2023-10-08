using System.Threading;
using System.Threading.Tasks;
using pdq.common.Connections;
using pdq.common.Logging;

namespace pdq.sqlserver
{
    public class SqlServerConnectionFactory : ConnectionFactory
    {
        public SqlServerConnectionFactory(ILoggerProxy logger)
            : base(logger)
        {
        }

        protected override Task<IConnection> ConstructConnectionAsync(IConnectionDetails connectionDetails, CancellationToken cancellationToken = default)
        {
            var conn = new SqlServerConnection(this.logger, connectionDetails);
            return Task.FromResult(conn as IConnection);
        }
    }
}

