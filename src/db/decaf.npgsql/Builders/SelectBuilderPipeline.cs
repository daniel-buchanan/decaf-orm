using decaf.common.Utilities;
using decaf.db.common.Builders;

namespace decaf.npgsql.Builders
{
    public class SelectBuilderPipeline : db.common.ANSISQL.SelectBuilderPipeline
    {
        protected override bool LimitBeforeGroupBy => false;

        public SelectBuilderPipeline(
            DecafOptions options,
            IHashProvider hashProvider,
            IQuotedIdentifierBuilder quotedIdentifierBuilder,
            db.common.Builders.IWhereBuilder whereBuilder,
            IConstants constants)
            : base(options, hashProvider, quotedIdentifierBuilder, whereBuilder, constants)
        {
        }
    }
}
