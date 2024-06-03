using decaf.common.Templates;
using decaf.db.common.Builders;
using decaf.state;

namespace decaf.tests.common.Mocks
{
    public class MockUpdatePipeline : db.common.ANSISQL.UpdateBuilderPipeline
    {
        public MockUpdatePipeline(
            DecafOptions options,
            IConstants constants,
            IParameterManager parameterManager,
            db.common.Builders.IWhereBuilder whereBuilder,
            IQuotedIdentifierBuilder quotedIdentifierBuilder,
            IBuilderPipeline<ISelectQueryContext> selectBuilder) :
            base(options, constants, parameterManager, whereBuilder, quotedIdentifierBuilder, selectBuilder)
        {
        }
    }
}