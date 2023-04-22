using System;
using System.Data;
using System.Data.Common;

namespace pdq.tests.common.Mocks
{
    public class MockDbConnection : DbConnection
    {
        private ConnectionState state;

        public MockDbConnection()
        {
        }

        public override string ConnectionString { get; set; }

        public override int ConnectionTimeout => 0;

        public override string Database => String.Empty;

        public override ConnectionState State => state;

        public override string DataSource { get; }

        public override string ServerVersion { get; }

        public override void ChangeDatabase(string databaseName) { }

        public override void Close() => this.state = ConnectionState.Closed;

        public override void Open() => this.state = ConnectionState.Open;

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
            => new MockDbTransaction(this, isolationLevel);

        protected override DbCommand CreateDbCommand() => new MockDbCommand();
    }
}

