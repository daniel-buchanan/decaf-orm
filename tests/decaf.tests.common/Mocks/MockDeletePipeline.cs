using decaf.common.Templates;
using decaf.db.common.Builders;

namespace decaf.tests.common.Mocks
{
    public class MockDeletePipeline : db.common.ANSISQL.DeleteBuilderPipeline
    {
        public MockDeletePipeline(
            DecafOptions options,
            IConstants constants,
            IParameterManager parameterManager,
            db.common.Builders.IWhereBuilder whereBuilder,
            IQuotedIdentifierBuilder quotedIdentifierBuilder) :
            base(options, constants, parameterManager, whereBuilder, quotedIdentifierBuilder)
        {
        }
    }
}