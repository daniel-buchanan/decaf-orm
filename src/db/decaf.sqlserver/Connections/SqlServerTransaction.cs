using System;
using System.Data;
using decaf.common.Connections;
using decaf.common.Exceptions;
using decaf.common.Logging;

namespace decaf.sqlserver;

public class SqlServerTransaction(
    Guid id,
    ILoggerProxy logger,
    IConnection connection,
    DecafOptions options,
    SqlServerOptions npgsqlOptions)
    : Transaction(id, logger, connection, options)
{
    /// <inheritdoc/>
    /// <exception cref="InvalidConnectionException"></exception>
    public override IDbTransaction GetUnderlyingTransaction()
    {
        var sqlConnection = Connection as SqlServerConnection;
        if (sqlConnection == null)
            throw new InvalidConnectionException($"The provided connection for Transaction {Id} is not of the type \"NpgsqlConnection\"");

        return sqlConnection.GetUnderlyingConnection()
            .BeginTransaction(npgsqlOptions.TransactionIsolationLevel);
    }
}