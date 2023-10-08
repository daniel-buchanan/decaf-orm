using System.Threading;
using System.Threading.Tasks;
using pdq.common.Connections;
using pdq.common.Logging;

namespace pdq.tests.common.Mocks
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

