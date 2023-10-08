using System;
using System.Threading;
using System.Threading.Tasks;
using pdq.common.Connections;
using pdq.common.Logging;

namespace pdq.tests.common.Mocks
{
	public class MockTransactionFactory : TransactionFactory
	{
        public MockTransactionFactory(
            IConnectionFactory connectionFactory,
            ILoggerProxy logger,
            PdqOptions options)
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

