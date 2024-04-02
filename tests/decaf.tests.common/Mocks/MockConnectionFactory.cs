using System.Threading;
using System.Threading.Tasks;
using decaf.common.Connections;
using decaf.common.Logging;
using decaf.db.common;

namespace decaf.tests.common.Mocks
{
    public class MockConnectionFactory : ConnectionFactory
    {
        private readonly MockDatabaseOptions dbOptions;

        public MockConnectionFactory(IDatabaseOptions options, ILoggerProxy logger) : base(logger)
            => this.dbOptions = options as MockDatabaseOptions ?? new MockDatabaseOptions();

        protected override Task<IConnection> ConstructConnectionAsync(IConnectionDetails connectionDetails, CancellationToken cancellationToken = default)
        {
            var connection = (IConnection)new MockConnection(dbOptions.ThrowOnCommit, logger, connectionDetails);
            return Task.FromResult(connection);
        }
    }
}

