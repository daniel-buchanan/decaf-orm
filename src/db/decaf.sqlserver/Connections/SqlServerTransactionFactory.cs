using System;
using System.Threading;
using System.Threading.Tasks;
using decaf.common.Connections;
using decaf.common.Logging;

namespace decaf.sqlserver
{
    public class SqlServerTransactionFactory : TransactionFactory
    {
        private readonly SqlServerOptions npgsqlOptions;

        public SqlServerTransactionFactory(
            IConnectionFactory connectionFactory,
            ILoggerProxy logger,
            DecafOptions options,
            SqlServerOptions npgsqlOptions)
            : base(connectionFactory, logger, options)
        {
            this.npgsqlOptions = npgsqlOptions;
        }

        protected override Task<ITransaction> CreateTransactionAsync(IConnection connection, CancellationToken cancellationToken = default)
        {
            var transactionId = Guid.NewGuid();
            logger.Debug($"NpgsqlTransactionFactory :: Creating Transaction with Id: {transactionId}");

            var sqlServerTransaction = new SqlServerTransaction(
                transactionId,
                logger,
                connection,
                options,
                npgsqlOptions);

            var transaction = sqlServerTransaction as ITransaction;
            return Task.FromResult(transaction);
        }
    }
}

