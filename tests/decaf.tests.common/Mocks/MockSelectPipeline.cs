using decaf.common.Templates;
using decaf.db.common.Builders;

namespace decaf.tests.common.Mocks
{
    public class MockSelectPipeline : db.common.ANSISQL.SelectBuilderPipeline
    {
        public MockSelectPipeline(
            DecafOptions options,
            IConstants constants,
            IParameterManager parameterManager,
            IQuotedIdentifierBuilder quotedIdentifierBuilder,
            db.common.Builders.IWhereBuilder whereBuilder) :
            base(options, constants, parameterManager, quotedIdentifierBuilder, whereBuilder)
        {
        }
    }
}