using decaf.db.common;

namespace decaf.tests.common.Mocks
{
    public class MockDatabaseOptions : DatabaseOptions
    {
        public void ThrowExceptionOnCommit()
            => ThrowOnCommit = true;
        
        public bool ThrowOnCommit { get; protected set; }
    }
}