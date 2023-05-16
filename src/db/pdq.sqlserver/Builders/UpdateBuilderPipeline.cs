using System;
using System.Linq;
using pdq.common.Utilities;
using pdq.db.common.Builders;
using pdq.state;
using pdq.state.ValueSources.Update;

namespace pdq.sqlserver.Builders
{
    public class UpdateBuilderPipeline : db.common.ANSISQL.UpdateBuilderPipeline
	{

        public UpdateBuilderPipeline(
            PdqOptions options,
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

