using System;
using System.Threading.Tasks;
using pdq.common.Connections;
using pdq.common.Logging;

namespace pdq.sqlserver
{
    public class SqlServerTransactionFactory : TransactionFactory
    {
        private readonly SqlServerOptions npgsqlOptions;

        public SqlServerTransactionFactory(
            IConnectionFactory connectionFactory,
            ILoggerProxy logger,
            PdqOptions options,
            SqlServerOptions npgsqlOptions)
            : base(connectionFactory, logger, options)
        {
            this.npgsqlOptions = npgsqlOptions;
        }

        protected override Task<ITransaction> CreateTransaction(IConnection connection)
        {
            var transactionId = Guid.NewGuid();
            this.logger.Debug($"NpgsqlTransactionFactory :: Creating Transaction with Id: {transactionId}");

            var sqlServerTransaction = new SqlServerTransaction(
                transactionId,
                this.logger,
                connection,
                this.options,
                this.npgsqlOptions);

            var transaction = sqlServerTransaction as ITransaction;
            return Task.FromResult(transaction);
        }
    }
}

