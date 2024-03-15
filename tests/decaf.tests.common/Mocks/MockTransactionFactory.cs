using System;
using System.Threading;
using System.Threading.Tasks;
using decaf.common.Connections;
using decaf.common.Logging;

namespace decaf.tests.common.Mocks
{
	public class MockTransactionFactory : TransactionFactory
	{
        public MockTransactionFactory(
            IConnectionFactory connectionFactory,
            ILoggerProxy logger,
            DecafOptions options)
            : base(connectionFactory, logger, options)
        {
        }

        protected override Task<ITransaction> CreateTransactionAsync(IConnection connection, CancellationToken cancellationToken = default)
        {
            var transaction = (ITransaction)new MockTransaction(Guid.NewGuid(), this.logger, connection, options);
            return Task.FromResult(transaction);
        }
    }
}

