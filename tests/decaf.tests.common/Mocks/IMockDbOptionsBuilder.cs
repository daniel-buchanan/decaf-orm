using System.Data;
using decaf.common.Connections;
using decaf.db.common;
using decaf.tests.common.Mocks;

namespace decaf.sqlserver
{
    public interface IMockDbOptionsBuilder :
        ISqlOptionsBuilder<MockDatabaseOptions, IMockDbOptionsBuilder, IConnectionDetails>
    {
        IMockDbOptionsBuilder ThrowOnCommit();
    }
}

