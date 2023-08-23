using System.Threading;
using System.Threading.Tasks;
using pdq.common.Connections;
using pdq.common.Logging;

namespace pdq.npgsql
{
    public class NpgsqlConnectionFactory : ConnectionFactory
    {
        public NpgsqlConnectionFactory(ILoggerProxy logger)
            : base(logger)
        {
        }

        protected override Task<IConnection> ConstructConnectionAsync(IConnectionDetails connectionDetails, CancellationToken cancellationToken = default)
        {
            var conn = new NpgsqlConnection(this.logger, connectionDetails);
            return Task.FromResult(conn as IConnection);
        }
    }
}

