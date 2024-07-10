using System;
using System.Data;
using decaf.common.Connections;
using decaf.common.Exceptions;
using decaf.common.Logging;

namespace decaf.npgsql
{
	public class NpgsqlTransaction : Transaction
	{
        private readonly NpgsqlOptions npgsqlOptions;

        public NpgsqlTransaction(
            Guid id,
            ILoggerProxy logger,
            IConnection connection,
            DecafOptions options,
            NpgsqlOptions npgsqlOptions)
            : base(id, logger, connection, options)
        {
            this.npgsqlOptions = npgsqlOptions;
        }

        /// <inheritdoc/>
        /// <exception cref="InvalidConnectionException"></exception>
        public override IDbTransaction GetUnderlyingTransaction()
        {
            var npgsqlConnection = connection as NpgsqlConnection;
            if (npgsqlConnection == null)
                throw new InvalidConnectionException($"The provided connection for Transaction {Id} is not of the type \"NpgsqlConnection\"");

            return npgsqlConnection.GetUnderlyingConnection()
                .BeginTransaction(npgsqlOptions.TransactionIsolationLevel);
        }
    }
}

