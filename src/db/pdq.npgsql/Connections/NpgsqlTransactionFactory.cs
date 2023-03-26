using System;
using System.Threading.Tasks;
using pdq.common.Connections;
using pdq.common.Logging;

namespace pdq.npgsql
{
    public class NpgsqlTransactionFactory : TransactionFactory
    {
        private readonly NpgsqlOptions npgsqlOptions;

        public NpgsqlTransactionFactory(
            IConnectionFactory connectionFactory,
            ILoggerProxy logger,
            PdqOptions options,
            NpgsqlOptions npgsqlOptions)
            : base(connectionFactory, logger, options)
        {
            this.npgsqlOptions = npgsqlOptions;
        }

        protected override Task<ITransaction> CreateTransaction(IConnection connection)
        {
            var transactionId = Guid.NewGuid();
            this.logger.Debug($"NpgsqlTransactionFactory :: Creating Transaction with Id: {transactionId}");

            var npgsqlTransaction = new NpgsqlTransaction(
                transactionId,
                this.logger,
                connection,
                this.options,
                this.npgsqlOptions);

            var transaction = npgsqlTransaction as ITransaction;
            return Task.FromResult(transaction);
        }
    }
}

