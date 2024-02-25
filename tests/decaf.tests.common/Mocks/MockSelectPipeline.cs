using decaf.common.Utilities;
using decaf.db.common.Builders;

namespace decaf.tests.common.Mocks
{
    public class MockSelectPipeline : db.common.ANSISQL.SelectBuilderPipeline
    {
        public MockSelectPipeline(
            DecafOptions options,
            IHashProvider hashProvider,
            IQuotedIdentifierBuilder quotedIdentifierBuilder,
            db.common.Builders.IWhereBuilder whereBuilder,
            IConstants constants) :
            base(options, hashProvider, quotedIdentifierBuilder, whereBuilder, constants)
        {
        }
    }
}