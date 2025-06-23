using System;
using System.Threading;
using System.Threading.Tasks;
using decaf.common.Connections;
using decaf.common.Logging;

namespace decaf.npgsql;

public class NpgsqlTransactionFactory : TransactionFactory
{
    private readonly NpgsqlOptions npgsqlOptions;

    public NpgsqlTransactionFactory(
        IConnectionFactory connectionFactory,
        ILoggerProxy logger,
        DecafOptions options,
        NpgsqlOptions npgsqlOptions)
        : base(connectionFactory, logger, options)
    {
        this.npgsqlOptions = npgsqlOptions;
    }

    protected override Task<ITransaction> CreateTransactionAsync(IConnection connection, CancellationToken cancellationToken = default)
    {
        var transactionId = Guid.NewGuid();
        logger.Debug($"NpgsqlTransactionFactory :: Creating Transaction with Id: {transactionId}");

        var npgsqlTransaction = new NpgsqlTransaction(
            transactionId,
            logger,
            connection,
            options,
            npgsqlOptions);

        var transaction = npgsqlTransaction as ITransaction;
        return Task.FromResult(transaction);
    }
}