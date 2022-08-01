using System;
using System.Data;

namespace pdq.core_tests.Mocks
{
    public class MockIDbConnection : IDbConnection
    {
        private ConnectionState state;

        public MockIDbConnection()
        {
        }

        public string ConnectionString { get; set; }

        public int ConnectionTimeout => 0;

        public string Database => String.Empty;

        public ConnectionState State => state;

        public IDbTransaction BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            throw new NotImplementedException();
        }

        public void ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        public void Close() => this.state = ConnectionState.Closed;

        public IDbCommand CreateCommand()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // nothing to do here
        }

        public void Open() => this.state = ConnectionState.Open;
    }
}

