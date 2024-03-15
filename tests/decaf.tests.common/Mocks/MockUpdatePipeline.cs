using decaf.common.Utilities;
using decaf.db.common.Builders;
using decaf.state;

namespace decaf.tests.common.Mocks
{
    public class MockUpdatePipeline : db.common.ANSISQL.UpdateBuilderPipeline
    {
        public MockUpdatePipeline(
            DecafOptions options,
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