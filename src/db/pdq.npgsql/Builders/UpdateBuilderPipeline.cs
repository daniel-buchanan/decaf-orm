using System;
using System.Linq;
using pdq.common.Templates;
using pdq.common.Utilities;
using pdq.db.common;
using pdq.db.common.Builders;
using pdq.Exceptions;
using pdq.state;
using pdq.state.ValueSources.Update;

namespace pdq.npgsql.Builders
{
	public class UpdateBuilderPipeline : db.common.ANSISQL.UpdateBuilderPipeline
	{
        public UpdateBuilderPipeline(
            PdqOptions options,
            NpgsqlOptions dbOptions,
            IHashProvider hashProvider,
            db.common.Builders.IWhereBuilder whereBuilder,
            IQuotedIdentifierBuilder quotedIdentifierBuilder,
            IBuilderPipeline<ISelectQueryContext> selectBuilder,
            IConstants constants)
            : base(options, dbOptions, hashProvider, whereBuilder, quotedIdentifierBuilder, selectBuilder, constants)
        {
        }
    }
}

