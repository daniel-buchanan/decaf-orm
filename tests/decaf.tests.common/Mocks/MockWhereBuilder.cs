using decaf.db.common.Builders;

namespace decaf.tests.common.Mocks
{
    public class MockWhereBuilder : db.common.ANSISQL.WhereBuilder
    {
        public MockWhereBuilder(
            IQuotedIdentifierBuilder quotedIdentifierBuilder,
            IConstants constants) :
            base(quotedIdentifierBuilder, constants)
        {
        }
    }
}