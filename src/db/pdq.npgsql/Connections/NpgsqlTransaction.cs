using System;
using System.Data;
using pdq.common.Connections;
using pdq.common.Logging;
using pdq.common.Exceptions;

namespace pdq.npgsql
{
	public class NpgsqlTransaction : Transaction
	{
        private readonly NpgsqlOptions npgsqlOptions;

        public NpgsqlTransaction(
            Guid id,
            ILoggerProxy logger,
            IConnection connection,
            PdqOptions options,
            NpgsqlOptions npgsqlOptions)
            : base(id, logger, connection, options)
        {
            this.npgsqlOptions = npgsqlOptions;
        }

        /// <inheritdoc/>
        /// <exception cref="InvalidConnectionException"></exception>
        public override IDbTransaction GetUnderlyingTransaction()
        {
            var npgsqlConnection = this.connection as NpgsqlConnection;
            if (npgsqlConnection == null)
                throw new InvalidConnectionException($"The provided connection for Transaction {this.Id} is not of the type \"NpgsqlConnection\"");

            return npgsqlConnection.GetUnderlyingConnection()
                .BeginTransaction(this.npgsqlOptions.TransactionIsolationLevel);
        }
    }
}

