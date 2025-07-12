using System.Threading;
using System.Threading.Tasks;
using decaf.common.Connections;
using decaf.common.Logging;
using decaf.db.common;

namespace decaf.tests.common.Mocks;

public class MockConnectionFactory(IDatabaseOptions options, ILoggerProxy logger) : ConnectionFactory(logger)
{
    private readonly MockDatabaseOptions dbOptions = options as MockDatabaseOptions ?? new MockDatabaseOptions();

    protected override Task<IConnection> ConstructConnectionAsync(IConnectionDetails connectionDetails, CancellationToken cancellationToken = default)
    {
        IConnection conn = new MockConnection(dbOptions, connectionDetails);
        return Task.FromResult(conn);
    }
}