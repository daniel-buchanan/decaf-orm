using decaf.common.Connections;
using decaf.db.common;

namespace decaf.tests.common.Mocks;

public class MockDbOptionsBuilder :
    SqlOptionsBuilder<MockDatabaseOptions, IMockDbOptionsBuilder, IConnectionDetails>,
    IMockDbOptionsBuilder
{
    public IMockDbOptionsBuilder ThrowOnCommit()
        => ConfigureProperty(nameof(MockDatabaseOptions.ThrowOnCommit), true);

    public IMockDbOptionsBuilder ThrowOnRollback()
        => ConfigureProperty(nameof(MockDatabaseOptions.ThrowOnRollback), true);

    public IMockDbOptionsBuilder Noop() => this;

    public override IMockDbOptionsBuilder WithConnectionString(string connectionString)
    {
        throw new System.NotImplementedException();
    }
}