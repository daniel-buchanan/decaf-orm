using System.Linq;
using decaf.db.common.Builders;
using decaf.common;
using decaf.common.Templates;
using decaf.state.Conditionals;

namespace decaf.npgsql.Builders
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

