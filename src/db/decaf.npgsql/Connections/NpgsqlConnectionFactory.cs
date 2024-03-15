using System.Threading;
using System.Threading.Tasks;
using decaf.common.Connections;
using decaf.common.Logging;

namespace decaf.npgsql
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

