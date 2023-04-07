using System;
using System.Data;
using pdq.common;
using pdq.common.Connections;
using pdq.common.Logging;

namespace pdq.npgsql
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

