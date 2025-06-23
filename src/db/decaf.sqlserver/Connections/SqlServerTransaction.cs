using System;
using System.Data;
using decaf.common.Connections;
using decaf.common.Exceptions;
using decaf.common.Logging;

namespace decaf.sqlserver;

public class SqlServerTransaction : Transaction
{
    private readonly SqlServerOptions sqlServerOptions;

    public SqlServerTransaction(
        Guid id,
        ILoggerProxy logger,
        IConnection connection,
        DecafOptions options,
        SqlServerOptions npgsqlOptions)
        : base(id, logger, connection, options)
    {
        sqlServerOptions = npgsqlOptions;
    }

    /// <inheritdoc/>
    /// <exception cref="InvalidConnectionException"></exception>
    public override IDbTransaction GetUnderlyingTransaction()
    {
        var sqlConnection = connection as SqlServerConnection;
        if (sqlConnection == null)
            throw new InvalidConnectionException($"The provided connection for Transaction {Id} is not of the type \"NpgsqlConnection\"");

        return sqlConnection.GetUnderlyingConnection()
            .BeginTransaction(sqlServerOptions.TransactionIsolationLevel);
    }
}