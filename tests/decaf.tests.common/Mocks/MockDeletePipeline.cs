using decaf.common.Utilities;
using decaf.db.common.Builders;

namespace decaf.tests.common.Mocks
{
    public class MockDeletePipeline : db.common.ANSISQL.DeleteBuilderPipeline
    {
        public MockDeletePipeline(
            DecafOptions options,
            IHashProvider hashProvider,
            db.common.Builders.IWhereBuilder whereBuilder,
            IQuotedIdentifierBuilder quotedIdentifierBuilder,
            IConstants constants) :
            base(options, hashProvider, whereBuilder, quotedIdentifierBuilder, constants)
        {
        }
    }
}