using decaf.common.Utilities;
using decaf.db.common.Builders;
using decaf.state;

namespace decaf.npgsql.Builders
{
	public class UpdateBuilderPipeline : db.common.ANSISQL.UpdateBuilderPipeline
	{
        public UpdateBuilderPipeline(
            DecafOptions options,
            IHashProvider hashProvider,
            db.common.Builders.IWhereBuilder whereBuilder,
            IQuotedIdentifierBuilder quotedIdentifierBuilder,
            IBuilderPipeline<ISelectQueryContext> selectBuilder,
            IConstants constants)
            : base(options, hashProvider, whereBuilder, quotedIdentifierBuilder, selectBuilder, constants)
        {
        }
    }
}

