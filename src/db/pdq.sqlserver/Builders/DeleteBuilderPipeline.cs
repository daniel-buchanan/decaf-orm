using System.Linq;
using pdq.common.Utilities;
using pdq.db.common.Builders;
using pdq.state;

namespace pdq.sqlserver.Builders
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

