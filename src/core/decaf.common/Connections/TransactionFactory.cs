using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using decaf.common.Logging;
using decaf.common.Utilities;

namespace decaf.common.Connections;

public abstract class TransactionFactory : ITransactionFactory
{
    private readonly Dictionary<string, ITransaction> transactions;
    private readonly IConnectionFactory connectionFactory;
    protected readonly ILoggerProxy logger;
    protected readonly DecafOptions options;

    protected TransactionFactory(
        IConnectionFactory connectionFactory,
        ILoggerProxy logger,
        DecafOptions options)
    {
        transactions = new Dictionary<string, ITransaction>();
        this.connectionFactory = connectionFactory;
        this.logger = logger;
        this.options = options;
    }

    /// <inheritdoc/>
    public ITransaction Get(IConnectionDetails connectionDetails)
        => GetAsync(connectionDetails).WaitFor();

    /// <inheritdoc/>
    public async Task<ITransaction> GetAsync(IConnectionDetails connectionDetails, CancellationToken cancellationToken = default)
    {
        logger.Debug($"ITransactionFactory :: Getting Transaction");
        var connectionString = await connectionDetails.GetConnectionStringAsync(cancellationToken);
        var key = connectionString.ToBase64String();
        var existing = transactions.TryGetValue(key, out var transaction);
        if (existing) return transaction;

        logger.Debug($"ITransactionFactory :: Creating new Transaction");
        var connection = await connectionFactory.GetConnectionAsync(connectionDetails, cancellationToken);
        if(connection.State != ConnectionState.Open && 
           !options.LazyInitialiseConnections)
            connection.Open();
            
        transaction = await CreateTransactionAsync(connection, cancellationToken);
        transactions.Add(key, transaction);

        return transaction;
    }
        
    protected abstract Task<ITransaction> CreateTransactionAsync(IConnection connection, CancellationToken cancellationToken = default);
}