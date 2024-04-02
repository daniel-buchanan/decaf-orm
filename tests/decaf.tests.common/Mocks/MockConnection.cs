using System.Data;
using decaf.common.Connections;
using decaf.common.Logging;

namespace decaf.tests.common.Mocks
{
	public class MockConnection : Connection
    {
        private readonly MockDatabaseOptions options;
        
        public MockConnection(
            MockDatabaseOptions options,
            ILoggerProxy logger, 
            IConnectionDetails connectionDetails)
            : base(logger, connectionDetails) 
            => this.options = options;

        public override IDbConnection GetUnderlyingConnection() => new MockDbConnection(options);
    }
}

