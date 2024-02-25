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
        public MockConnection(ILoggerProxy logger, IConnectionDetails connectionDetails)
            : base(logger, connectionDetails)
        {
        }

        public override IDbConnection GetUnderlyingConnection() => new MockDbConnection();
    }
}

