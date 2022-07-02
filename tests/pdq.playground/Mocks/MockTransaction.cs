using System;
using System.Data;
using Moq;
using pdq.common.Connections;
using pdq.common.Logging;

namespace pdq.playground.Mocks
{
    public class MockTransaction : Transaction
    {
        public MockTransaction(Guid id, ILoggerProxy logger, IConnection connection)
            : base(id, logger, connection)
        {
        }

        public override IDbTransaction GetUnderlyingTransaction()
        {
            var transaction = new Mock<IDbTransaction>();
            return transaction.Object;
        }
    }
}

