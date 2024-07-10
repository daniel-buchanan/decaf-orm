using decaf.common;
using decaf.db.common.Builders;
using decaf.state;

namespace decaf.db.common.ANSISQL
{
	public class QuotedIdentifierBuilder : IQuotedIdentifierBuilder
	{
        protected readonly string quote = string.Empty;
        protected readonly IConstants constants;

        public QuotedIdentifierBuilder(
            IDatabaseOptions options,
            IConstants constants)
        {
            this.constants = constants;
            if (options.QuotedIdentifiers)
                quote = constants.ColumnQuote;
        }

        public virtual void AddSelect(Column column, ISqlBuilder sqlBuilder)
        {
            if (!string.IsNullOrWhiteSpace(column.Source?.Alias))
                sqlBuilder.Append("{0}{1}{0}.", quote, column.Source.Alias);

            if (string.IsNullOrWhiteSpace(column.NewName))
                sqlBuilder.Append("{0}{1}{0}", quote, column.Name);
            else
                sqlBuilder.Append("{0}{1}{0} as {0}{2}{0}", quote, column.Name, column.NewName);
        }

        public virtual void AddColumn(Column column, ISqlBuilder sqlBuilder)
        {
            if (!string.IsNullOrWhiteSpace(column.Source?.Alias))
                sqlBuilder.Append("{0}{1}{0}.", quote, column.Source.Alias);

            sqlBuilder.Append("{0}{1}{0}", quote, column.Name);
        }

        public virtual void AddOutput(Output output, ISqlBuilder sqlBuilder)
            => sqlBuilder.Append("{0}{1}{0}", quote, output.Name);

        public virtual void AddFromTable(ITableTarget tableTarget, ISqlBuilder sqlBuilder)
        {
            var format = string.Empty;

            if (!string.IsNullOrWhiteSpace(tableTarget.Schema))
                format = "{0}{1}{0}.";

            if (!string.IsNullOrWhiteSpace(tableTarget.Alias))
                format = format + "{0}{2}{0} as {0}{3}{0}";

            sqlBuilder.Append(format, quote, tableTarget.Schema, tableTarget.Name, tableTarget.Alias);
        }

        public virtual void AddFromTable(string table, ISqlBuilder sqlBuilder)
        {
            sqlBuilder.Append("{0}{1}{0}", quote, table);
        }

        public virtual void AddClosingFromQuery(string alias, ISqlBuilder sqlBuilder)
        {
            sqlBuilder.PrependIndent();
            sqlBuilder.Append("{0} as {1}{2}{1}", constants.ClosingParen, quote, alias);
        }

        public virtual void AddOrderBy(OrderBy orderBy, ISqlBuilder sqlBuilder)
        {
            var alias = string.Empty;
            var formatStr = "{0}{2}{0} {3}";
            var order = orderBy.Order == SortOrder.Ascending ? constants.Ascending : constants.Descending;
            if (!string.IsNullOrWhiteSpace(orderBy.Source.Alias))
            {
                alias = orderBy.Source.Alias;
                formatStr = "{0}{1}{0}." + formatStr;
            }
            else
            {
                formatStr = "{1}" + formatStr;
            }

            sqlBuilder.Append(formatStr, quote, alias, orderBy.Name, order);
        }

        public virtual void AddGroupBy(GroupBy groupBy, ISqlBuilder sqlBuilder)
        {
            var alias = string.Empty;
            var formatStr = "{0}{2}{0} {3}";
            if (!string.IsNullOrWhiteSpace(groupBy.Source.Alias))
            {
                alias = groupBy.Source.Alias;
                formatStr = "{0}{1}{0}." + formatStr;
            }
            else
            {
                formatStr = "{1}" + formatStr;
            }

            sqlBuilder.Append(formatStr, quote, alias, groupBy.Name);
        }
    }
}

