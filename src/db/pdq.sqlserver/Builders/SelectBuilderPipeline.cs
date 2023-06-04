using System.Linq;
using pdq.common.Utilities;
using pdq.db.common.Builders;
using pdq.state;

namespace pdq.sqlserver.Builders
{
    public class SelectBuilderPipeline : db.common.ANSISQL.SelectBuilderPipeline
    {
        protected override bool LimitBeforeGroupBy => true;

        public SelectBuilderPipeline(
            PdqOptions options,
            IHashProvider hashProvider,
            IQuotedIdentifierBuilder quotedIdentifierBuilder,
            db.common.Builders.IWhereBuilder whereBuilder,
            IConstants constants)
            : base(options, hashProvider, quotedIdentifierBuilder, whereBuilder, constants)
        {
        }
    }
}

