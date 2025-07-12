using System.Data;
using decaf.common.Connections;
using decaf.common.Logging;

namespace decaf.tests.common.Mocks;

public class MockConnection(
    MockDatabaseOptions options,
    IConnectionDetails connectionDetails)
    : Connection(connectionDetails)
{
    public override IDbConnection GetUnderlyingConnection() => new MockDbConnection(options);
}