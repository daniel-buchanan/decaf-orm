using System.Data;
using decaf.common.Connections;
using decaf.db.common;
using decaf.tests.common.Mocks;

namespace decaf.sqlserver
{
    public class MockDbOptionsBuilder :
        SqlOptionsBuilder<MockDatabaseOptions, IMockDbOptionsBuilder, IConnectionDetails>,
		IMockDbOptionsBuilder
	{
        public IMockDbOptionsBuilder ThrowOnCommit()
            => ConfigureProperty(nameof(MockDatabaseOptions.ThrowOnCommit), true);

        public override IMockDbOptionsBuilder WithConnectionString(string connectionString)
        {
            throw new System.NotImplementedException();
        }
    }
}

