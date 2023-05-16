using System;
using System.Linq;
using pdq.common.Templates;
using pdq.common.Utilities;
using pdq.db.common;
using pdq.db.common.Builders;
using pdq.state;
using pdq.state.QueryTargets;

namespace pdq.npgsql.Builders
{
    public class DeleteBuilderPipeline : db.common.ANSISQL.DeleteBuilderPipeline
    {
        public DeleteBuilderPipeline(
            PdqOptions options,
            IHashProvider hashProvider,
            db.common.Builders.IWhereBuilder whereBuilder,
            IQuotedIdentifierBuilder quotedIdentifierBuilder,
            IConstants constants)
            : base(options, hashProvider, whereBuilder, quotedIdentifierBuilder, constants)
        {
            
        }
    }
}

