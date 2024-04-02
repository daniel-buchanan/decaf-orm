using decaf.common.Connections;
using decaf.db.common;

namespace decaf.tests.common.Mocks
{
    public interface IMockDbOptionsBuilder :
        ISqlOptionsBuilder<MockDatabaseOptions, IMockDbOptionsBuilder, IConnectionDetails>
    {
        IMockDbOptionsBuilder ThrowOnCommit();

        IMockDbOptionsBuilder ThrowOnRollback();

        IMockDbOptionsBuilder Noop();
    }
}

