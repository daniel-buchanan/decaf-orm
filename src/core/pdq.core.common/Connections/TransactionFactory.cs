using System;
using System.Collections.Generic;
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

        public TransactionFactory(
            IConnectionFactory connectionFactory,
            ILoggerProxy logger)
        {
            this.transactions = new Dictionary<string, ITransaction>();
            this.connectionFactory = connectionFactory;
            this.logger = logger;
        }

        public ITransaction Get(IConnectionDetails connection)
            => GetAsync(connection).WaitFor();

        /// <inheritdoc/>
        public async Task<ITransaction> GetAsync(IConnectionDetails connectionDetails)
        {
            this.logger.Debug($"ITransactionFactory :: Getting Transaction");
            var key = (await connectionDetails.GetConnectionStringAsync()).ToBase64String();
            var existing = this.transactions.TryGetValue(key, out var transaction);
            if (existing) return transaction;

            this.logger.Debug($"ITransactionFactory :: Creating new Transaction");
            var connection = await this.connectionFactory.GetAsync(connectionDetails);
            transaction = await CreateTransaction(connection);
            this.transactions.Add(key, transaction);

            return transaction;
        }

        /// <inheritdoc/>
        protected abstract Task<ITransaction> CreateTransaction(IConnection connection);
    }
}

