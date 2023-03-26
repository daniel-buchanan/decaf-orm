using System;
using System.Threading.Tasks;
using pdq.common;
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

        protected override Task<IConnection> ConstructConnection(IConnectionDetails connectionDetails)
        {
            var conn = new NpgsqlConnection(this.logger, connectionDetails);
            return Task.FromResult(conn as IConnection);
        }
    }
}

