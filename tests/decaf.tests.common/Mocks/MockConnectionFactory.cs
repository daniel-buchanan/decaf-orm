using System.Threading;
using System.Threading.Tasks;
using decaf.common.Connections;
using decaf.common.Logging;

namespace decaf.tests.common.Mocks
{
    public class MockConnectionFactory : ConnectionFactory
    {
        public MockConnectionFactory(ILoggerProxy logger) : base(logger)
        {
        }

        protected override Task<IConnection> ConstructConnectionAsync(IConnectionDetails connectionDetails, CancellationToken cancellationToken = default)
        {
            var connection = (IConnection)new MockConnection(this.logger, connectionDetails);
            return Task.FromResult(connection);
        }
    }
}

