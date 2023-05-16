using System;
using System.Linq;
using pdq.common.Utilities;
using pdq.db.common.Builders;
using pdq.state;

namespace pdq.npgsql.Builders
{
    public class SelectBuilderPipeline : db.common.ANSISQL.SelectBuilderPipeline
    {
        protected override bool LimitBeforeGroupBy => false;

        public SelectBuilderPipeline(
            PdqOptions options,
            NpgsqlOptions dbOptions,
            IHashProvider hashProvider,
            IQuotedIdentifierBuilder quotedIdentifierBuilder,
            db.common.Builders.IWhereBuilder whereBuilder,
            IConstants constants)
            : base(options, dbOptions, hashProvider, quotedIdentifierBuilder, whereBuilder, constants)
        {
        }
    }
}
