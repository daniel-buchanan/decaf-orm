using System.Linq;
using pdq.common;
using pdq.common.Templates;
using pdq.db.common.Builders;
using pdq.state.Conditionals;

namespace pdq.sqlserver.Builders
{
    public class WhereBuilder : db.common.ANSISQL.WhereBuilder
    {
        public WhereBuilder(
            IQuotedIdentifierBuilder quotedIdentifierBuilder,
            IConstants constants)
            : base(quotedIdentifierBuilder, constants)
        {
        }
    }
}

