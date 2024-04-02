using System;
using System.Data;
using System.Data.Common;

namespace decaf.tests.common.Mocks
{
    public class MockDbTransaction : DbTransaction
    {
        private readonly bool throwOnCommit;
        private readonly DbConnection connection;
        private readonly IsolationLevel isolationLevel;

        public MockDbTransaction(bool throwOnCommit, DbConnection connection, IsolationLevel il)
        {
            this.throwOnCommit = throwOnCommit;
            this.connection = connection;
            this.isolationLevel = il;
        }

        public override IsolationLevel IsolationLevel => this.isolationLevel;

        protected override DbConnection DbConnection => this.connection;

        public override void Commit()
        {
            if (throwOnCommit) throw new NullReferenceException("Throwing on Commit!");
        }

        public override void Rollback() { }
    }
}

