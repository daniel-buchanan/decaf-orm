using System;
using System.Threading;
using System.Threading.Tasks;
using decaf.common.Connections;
using decaf.common.Logging;
using decaf.db.common;

namespace decaf.tests.common.Mocks
{
	public class MockTransactionFactory : TransactionFactory
    {
        private readonly MockDatabaseOptions dbOptions;

        public MockTransactionFactory(
            IDatabaseOptions dbOptions,
            IConnectionFactory connectionFactory,
            ILoggerProxy logger,
            DecafOptions options)
            : base(connectionFactory, logger, options)
        {
            this.dbOptions = dbOptions as MockDatabaseOptions ?? new MockDatabaseOptions();
        }

        protected override Task<ITransaction> CreateTransactionAsync(IConnection connection, CancellationToken cancellationToken = default)
        {
            var transaction = (ITransaction)new MockTransaction(Guid.NewGuid(), dbOptions.ThrowOnCommit, this.logger, connection, options);
            return Task.FromResult(transaction);
        }
    }
}

