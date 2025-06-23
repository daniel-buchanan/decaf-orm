using decaf.db.common;

namespace decaf.tests.common.Mocks;

public class MockDatabaseOptions : DatabaseOptions
{
    public bool ThrowOnCommit { get; protected set; }
        
    public bool ThrowOnRollback { get; protected set; }
}