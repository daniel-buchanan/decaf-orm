using System;
using System.Data;
using Moq;
using pdq.common;
using pdq.common.Connections;
using pdq.common.Logging;

namespace pdq.playground.Mocks
{
	public class MockConnection : Connection
	{
        public MockConnection(ILoggerProxy logger, IConnectionDetails connectionDetails)
            : base(logger, connectionDetails)
        {
        }

        public override IDbConnection GetUnderlyingConnection()
        {
            var connection = new Mock<IDbConnection>();
            return connection.Object;
        }
    }
}

