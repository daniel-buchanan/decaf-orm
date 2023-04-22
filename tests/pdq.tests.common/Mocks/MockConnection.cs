using System;
using System.Data;
using Moq;
using pdq.common;
using pdq.common.Connections;
using pdq.common.Logging;

namespace pdq.tests.common.Mocks
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

