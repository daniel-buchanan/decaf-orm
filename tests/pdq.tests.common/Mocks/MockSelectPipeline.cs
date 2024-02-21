using pdq.common.Utilities;
using pdq.db.common.Builders;

namespace pdq.tests.common.Mocks
{
    public class MockSelectPipeline : db.common.ANSISQL.SelectBuilderPipeline
    {
        public MockSelectPipeline(
            PdqOptions options,
            IHashProvider hashProvider,
            IQuotedIdentifierBuilder quotedIdentifierBuilder,
            db.common.Builders.IWhereBuilder whereBuilder,
            IConstants constants) :
            base(options, hashProvider, quotedIdentifierBuilder, whereBuilder, constants)
        {
        }
    }
}