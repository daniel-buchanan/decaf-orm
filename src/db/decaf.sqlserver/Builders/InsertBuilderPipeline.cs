using decaf.db.common;
using decaf.db.common.Builders;
using decaf.state;
using decaf.common.Templates;

namespace decaf.sqlserver.Builders
{
	public class InsertBuilderPipeline : db.common.ANSISQL.InsertBuilderPipeline
	{
        public InsertBuilderPipeline(
            DecafOptions options,
            IConstants constants,
            IParameterManager parameterManager,
            IQuotedIdentifierBuilder quotedIdentifierBuilder,
            IValueParser valueParser,
            IBuilderPipeline<ISelectQueryContext> selectBuilder)
            : base(options, constants, parameterManager, quotedIdentifierBuilder, valueParser, selectBuilder)
        {
        }
    }
}

