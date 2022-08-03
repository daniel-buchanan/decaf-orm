using System;
using System.Data;
using System.Data.Common;
using pdq.common.Connections;
using pdq.common.Logging;

namespace pdq.core_tests.Mocks
{
    public class MockTransaction : Transaction
    {
        public MockTransaction(
            Guid id,
            ILoggerProxy logger,
            IConnection connection,
            PdqOptions options)
            : base(id, logger, connection, options)
        {
        }

        public override IDbTransaction GetUnderlyingTransaction()
        {
            var conn = this.Connection.GetUnderlyingConnection() as DbConnection;
            return new MockDbTransaction(conn, IsolationLevel.Serializable);
        }
    }
}

