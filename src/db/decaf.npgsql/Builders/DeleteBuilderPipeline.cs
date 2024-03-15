using System;
using System.Linq;
using decaf.common.Utilities;
using decaf.db.common.Builders;
using decaf.common.Templates;
using decaf.db.common;
using decaf.state;
using decaf.state.QueryTargets;

namespace decaf.npgsql.Builders
{
    public class DeleteBuilderPipeline : db.common.ANSISQL.DeleteBuilderPipeline
    {
        public DeleteBuilderPipeline(
            DecafOptions options,
            IHashProvider hashProvider,
            db.common.Builders.IWhereBuilder whereBuilder,
            IQuotedIdentifierBuilder quotedIdentifierBuilder,
            IConstants constants)
            : base(options, hashProvider, whereBuilder, quotedIdentifierBuilder, constants)
        {
            
        }
    }
}

