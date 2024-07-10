using decaf.db.common.Builders;

namespace decaf.sqlserver.Builders
{
    public class WhereClauseBuilder : db.common.ANSISQL.WhereClauseBuilder
    {
        public WhereClauseBuilder(
            IQuotedIdentifierBuilder quotedIdentifierBuilder,
            IConstants constants)
            : base(quotedIdentifierBuilder, constants)
        {
        }
    }
}

