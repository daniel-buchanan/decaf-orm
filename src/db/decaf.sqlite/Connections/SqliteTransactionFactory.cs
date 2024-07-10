using decaf.common.Connections;
using decaf.common.Logging;

namespace decaf.sqlite;

public class SqliteTransactionFactory : TransactionFactory
{
    public SqliteTransactionFactory(
        IConnectionFactory connectionFactory, 
        ILoggerProxy logger, 
        DecafOptions options) : 
        base(connectionFactory, logger, options) { }

    protected override Task<ITransaction> CreateTransactionAsync(
        IConnection connection, 
        CancellationToken cancellationToken = default)
    {
        var transactionId = Guid.NewGuid();
        logger.Debug($"SqliteTransactionFactory :: Creating Transaction with Id: {transactionId}");

        var sqliteTransaction = new SqliteTransaction(
            transactionId,
            logger,
            connection,
            options);

        var transaction = sqliteTransaction as ITransaction;
        return Task.FromResult(transaction);
    }
}