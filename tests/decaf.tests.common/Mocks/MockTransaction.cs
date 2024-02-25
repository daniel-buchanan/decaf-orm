using System;
using System.Data;
using System.Data.Common;
using decaf.common.Connections;
using decaf.common.Logging;

namespace decaf.tests.common.Mocks
{
    public class MockTransaction : Transaction
    {
        public MockTransaction(
            Guid id,
            ILoggerProxy logger,
            IConnection connection,
            DecafOptions options)
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

