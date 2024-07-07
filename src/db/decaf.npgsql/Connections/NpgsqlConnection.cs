using System.Data;
using decaf.common.Connections;
using decaf.common.Logging;

namespace decaf.npgsql
{
	public class NpgsqlConnection : Connection, IConnection
	{
        public NpgsqlConnection(
            ILoggerProxy logger,
            IConnectionDetails connectionDetails)
            : base(logger, connectionDetails)
        {
        }

        public override IDbConnection GetUnderlyingConnection()
        {
            var details = this.connectionDetails as INpgsqlConnectionDetails;
            return new Npgsql.NpgsqlConnection(details.GetConnectionString());
        }
    }
}

