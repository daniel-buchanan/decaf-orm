using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using pdq.common.Logging;
using pdq.common.Utilities;

namespace pdq.common.Connections
{
	public abstract class TransactionFactory : ITransactionFactory
	{
        private readonly Dictionary<string, ITransaction> transactions;
        private readonly IConnectionFactory connectionFactory;
        protected readonly ILoggerProxy logger;
        protected readonly PdqOptions options;

        protected TransactionFactory(
            IConnectionFactory connectionFactory,
            ILoggerProxy logger,
            PdqOptions options)
        {
            this.transactions = new Dictionary<string, ITransaction>();
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
            this.logger.Debug($"ITransactionFactory :: Getting Transaction");
            var connectionString = await connectionDetails.GetConnectionStringAsync(cancellationToken);
            var key = connectionString.ToBase64String();
            var existing = this.transactions.TryGetValue(key, out var transaction);
            if (existing) return transaction;

            this.logger.Debug($"ITransactionFactory :: Creating new Transaction");
            var connection = await this.connectionFactory.GetConnectionAsync(connectionDetails, cancellationToken);
            transaction = await CreateTransactionAsync(connection, cancellationToken);
            this.transactions.Add(key, transaction);

            return transaction;
        }

        /// <inheritdoc/>
        protected abstract Task<ITransaction> CreateTransactionAsync(IConnection connection, CancellationToken cancellationToken = default);
    }
}

