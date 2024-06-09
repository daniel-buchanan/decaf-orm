using decaf.common.Templates;
using decaf.db.common.Builders;
using decaf.state;

namespace decaf.sqlserver.Builders
{
    public class UpdateBuilderPipeline : db.common.ANSISQL.UpdateBuilderPipeline
	{

        public UpdateBuilderPipeline(
            DecafOptions options,
            IConstants constants,
            IParameterManager parameterManager,
            db.common.Builders.IWhereBuilder whereBuilder,
            IQuotedIdentifierBuilder quotedIdentifierBuilder,
            IBuilderPipeline<ISelectQueryContext> selectBuilder)
            : base(options, constants, parameterManager, whereBuilder, quotedIdentifierBuilder, selectBuilder)
        {
        }
    }
}

