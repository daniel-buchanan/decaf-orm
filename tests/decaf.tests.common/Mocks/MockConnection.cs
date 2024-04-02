using System;
using System.Data;
using decaf.common.Connections;
using decaf.common.Logging;
using Moq;
using decaf.common;

namespace decaf.tests.common.Mocks
{
	public class MockConnection : Connection
    {
        private readonly bool throwOnCommit;
        
        public MockConnection(
            bool throwOnCommit,
            ILoggerProxy logger, 
            IConnectionDetails connectionDetails)
            : base(logger, connectionDetails) 
            => this.throwOnCommit = throwOnCommit;

        public override IDbConnection GetUnderlyingConnection() => new MockDbConnection(throwOnCommit);
    }
}

