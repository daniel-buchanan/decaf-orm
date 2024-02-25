using System;
using decaf.db.common;
using decaf.db.common.Builders;
using decaf.state;

namespace decaf.sqlserver.Builders
{
    public class QuotedIdentifierBuilder : db.common.ANSISQL.QuotedIdentifierBuilder
    {
        public QuotedIdentifierBuilder(IDatabaseOptions options, IConstants constants)
            : base(options, constants)
        {
        }

        public override void AddFromTable(ITableTarget tableTarget, ISqlBuilder sqlBuilder)
        {
            var format = string.Empty;

            if (!string.IsNullOrWhiteSpace(tableTarget.Schema))
                format = "{0}{1}{0}.";

            if (!string.IsNullOrWhiteSpace(tableTarget.Alias))
                format += "{0}{2}{0} {0}{3}{0}";

            sqlBuilder.Append(format, base.quote, tableTarget.Schema, tableTarget.Name, tableTarget.Alias);
        }

        public override void AddClosingFromQuery(string alias, ISqlBuilder sqlBuilder)
        {
            sqlBuilder.PrependIndent();
            sqlBuilder.Append("{0} {1}{2}{1}", constants.ClosingParen, this.quote, alias);
        }
    }
}

