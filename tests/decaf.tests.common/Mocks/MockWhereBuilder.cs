using decaf.db.common.Builders;

namespace decaf.tests.common.Mocks
{
    public class MockWhereClauseBuilder : db.common.ANSISQL.WhereClauseBuilder
    {
        public MockWhereClauseBuilder(
            IQuotedIdentifierBuilder quotedIdentifierBuilder,
            IConstants constants) :
            base(quotedIdentifierBuilder, constants)
        {
        }
    }
}