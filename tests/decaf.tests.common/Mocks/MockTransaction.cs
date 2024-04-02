using System;
using System.Data;
using System.Data.Common;
using decaf.common.Connections;
using decaf.common.Logging;

namespace decaf.tests.common.Mocks
{
    public class MockTransaction : Transaction
    {
        private readonly MockDatabaseOptions dbOptions;
        
        public MockTransaction(
            Guid id,
            MockDatabaseOptions dbOptions,
            ILoggerProxy logger,
            IConnection connection,
            DecafOptions options)
            : base(id, logger, connection, options)
        {
            this.dbOptions = dbOptions;
        }

        public override IDbTransaction GetUnderlyingTransaction()
        {
            var conn = this.Connection.GetUnderlyingConnection() as DbConnection;
            return new MockDbTransaction(dbOptions, conn, System.Data.IsolationLevel.Serializable);
        }
    }
}

