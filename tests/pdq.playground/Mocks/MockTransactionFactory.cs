using System;
using pdq.common.Connections;
using pdq.common.Logging;

namespace pdq.playground.Mocks
{
	public class MockTransactionFactory : TransactionFactory
	{
        public MockTransactionFactory(IConnectionFactory connectionFactory, ILoggerProxy logger)
            : base(connectionFactory, logger)
        {
        }

        protected override Task<ITransaction> CreateTransaction(IConnection connection)
        {
            var transaction = (ITransaction)new MockTransaction(Guid.NewGuid(), this.logger, connection);
            return Task.FromResult(transaction);
        }
    }
}

