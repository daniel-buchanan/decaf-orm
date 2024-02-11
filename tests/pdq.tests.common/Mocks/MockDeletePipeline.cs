using pdq.common.Utilities;
using pdq.db.common.Builders;

namespace pdq.tests.common.Mocks
{
    public class MockDeletePipeline : db.common.ANSISQL.DeleteBuilderPipeline
    {
        public MockDeletePipeline(
            PdqOptions options,
            IHashProvider hashProvider,
            db.common.Builders.IWhereBuilder whereBuilder,
            IQuotedIdentifierBuilder quotedIdentifierBuilder,
            IConstants constants) :
            base(options, hashProvider, whereBuilder, quotedIdentifierBuilder, constants)
        {
        }
    }
}