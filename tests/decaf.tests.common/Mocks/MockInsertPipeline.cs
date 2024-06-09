using decaf.common.Templates;
using decaf.db.common;
using decaf.db.common.Builders;
using decaf.state;

namespace decaf.tests.common.Mocks
{
    public class MockInsertPipeline : db.common.ANSISQL.InsertBuilderPipeline
    {
        public MockInsertPipeline(
            DecafOptions options,
            IConstants constants,
            IParameterManager parameterManager,
            IQuotedIdentifierBuilder quotedIdentifierBuilder,
            IValueParser valueParser,
            IBuilderPipeline<ISelectQueryContext> selectBuilder) :
            base(options, constants, parameterManager, quotedIdentifierBuilder, valueParser, selectBuilder)
        {
        }
    }
}