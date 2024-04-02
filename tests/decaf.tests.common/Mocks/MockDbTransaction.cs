using System;
using System.Data;
using System.Data.Common;

namespace decaf.tests.common.Mocks
{
    public class MockDbTransaction : DbTransaction
    {
        private readonly MockDatabaseOptions options;
        private readonly DbConnection connection;
        private readonly IsolationLevel isolationLevel;

        public MockDbTransaction(MockDatabaseOptions options, DbConnection connection, IsolationLevel il)
        {
            this.options = options;
            this.connection = connection;
            this.isolationLevel = il;
        }

        public override IsolationLevel IsolationLevel => this.isolationLevel;

        protected override DbConnection DbConnection => this.connection;

        public override void Commit()
        {
            if (options.ThrowOnCommit) throw new NullReferenceException("Throwing on Commit!");
        }

        public override void Rollback()
        {
            if (options.ThrowOnRollback) throw new NullReferenceException("Throwing on Rollback!");
        }
    }
}

