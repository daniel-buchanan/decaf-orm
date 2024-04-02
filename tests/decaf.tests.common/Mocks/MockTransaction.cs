using System;
using System.Data;
using System.Data.Common;
using decaf.common.Connections;
using decaf.common.Logging;

namespace decaf.tests.common.Mocks
{
    public class MockTransaction : Transaction
    {
        private readonly bool throwOnCommit;
        
        public MockTransaction(
            Guid id,
            bool throwOnCommit,
            ILoggerProxy logger,
            IConnection connection,
            DecafOptions options)
            : base(id, logger, connection, options)
        {
            this.throwOnCommit = throwOnCommit;
        }

        public override IDbTransaction GetUnderlyingTransaction()
        {
            var conn = this.Connection.GetUnderlyingConnection() as DbConnection;
            return new MockDbTransaction(throwOnCommit, conn, IsolationLevel.Serializable);
        }
    }
}

