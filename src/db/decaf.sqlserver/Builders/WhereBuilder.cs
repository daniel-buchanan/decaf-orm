using decaf.db.common.Builders;

namespace decaf.sqlserver.Builders
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

