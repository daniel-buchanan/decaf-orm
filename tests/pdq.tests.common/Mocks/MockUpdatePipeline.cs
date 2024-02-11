using pdq.common.Utilities;
using pdq.db.common.Builders;
using pdq.state;

namespace pdq.tests.common.Mocks
{
    public class MockUpdatePipeline : db.common.ANSISQL.UpdateBuilderPipeline
    {
        public MockUpdatePipeline(
            PdqOptions options,
            IHashProvider hashProvider,
            db.common.Builders.IWhereBuilder whereBuilder,
            IQuotedIdentifierBuilder quotedIdentifierBuilder,
            IBuilderPipeline<ISelectQueryContext> selectBuilder,
            IConstants constants) :
            base(options, hashProvider, whereBuilder, quotedIdentifierBuilder, selectBuilder, constants)
        {
        }
    }
}