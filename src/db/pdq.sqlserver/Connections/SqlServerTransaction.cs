using System;
using System.Data;
using pdq.common.Connections;
using pdq.common.Logging;
using pdq.common.Exceptions;

namespace pdq.sqlserver
{
	public class SqlServerTransaction : Transaction
	{
        private readonly SqlServerOptions sqlServerOptions;

        public SqlServerTransaction(
            Guid id,
            ILoggerProxy logger,
            IConnection connection,
            PdqOptions options,
            SqlServerOptions npgsqlOptions)
            : base(id, logger, connection, options)
        {
            this.sqlServerOptions = npgsqlOptions;
        }

        /// <inheritdoc/>
        /// <exception cref="InvalidConnectionException"></exception>
        public override IDbTransaction GetUnderlyingTransaction()
        {
            var sqlConnection = this.connection as SqlServerConnection;
            if (sqlConnection == null)
                throw new InvalidConnectionException($"The provided connection for Transaction {this.Id} is not of the type \"NpgsqlConnection\"");

            return sqlConnection.GetUnderlyingConnection()
                .BeginTransaction(this.sqlServerOptions.TransactionIsolationLevel);
        }
    }
}

