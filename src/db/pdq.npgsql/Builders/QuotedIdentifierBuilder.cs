using pdq.db.common.Builders;
using pdq.state;

namespace pdq.npgsql.Builders
{
	public class QuotedIdentifierBuilder
	{
        private readonly string quote = string.Empty;

        public QuotedIdentifierBuilder(NpgsqlOptions options)
		{
            if (options.QuotedIdentifiers)
                this.quote = Constants.ColumnQuote;
        }

        public void AddSelect(Column column, ISqlBuilder sqlBuilder)
        {
            if (!string.IsNullOrWhiteSpace(column.Source?.Alias))
                sqlBuilder.Append("{0}{1}{0}.", this.quote, column.Source.Alias);

            if (string.IsNullOrWhiteSpace(column.NewName))
                sqlBuilder.Append("{0}{1}{0}", this.quote, column.Name);
            else
                sqlBuilder.Append("{0}{1}{0} as {0}{2}{0}", this.quote, column.Name, column.NewName);
        }

        public void AddColumn(Column column, ISqlBuilder sqlBuilder)
        {
            if (!string.IsNullOrWhiteSpace(column.Source?.Alias))
                sqlBuilder.Append("{0}{1}{0}.", this.quote, column.Source.Alias);
            
            sqlBuilder.Append("{0}{1}{0}", this.quote, column.Name);
        }

        public void AddOutput(Output output, ISqlBuilder sqlBuilder)
            => sqlBuilder.Append("{0}{1}{0}", this.quote, output.Name);

        public void AddFromTable(ITableTarget tableTarget, ISqlBuilder sqlBuilder)
        {
            var format = string.Empty;

            if (!string.IsNullOrWhiteSpace(tableTarget.Schema))
                format = "{0}{1}{0}.";

            if (!string.IsNullOrWhiteSpace(tableTarget.Alias))
                format = format + "{0}{2}{0} as {0}{3}{0}";

            sqlBuilder.Append(format, this.quote, tableTarget.Schema, tableTarget.Name, tableTarget.Alias);
        }

        public void AddFromTable(string table, ISqlBuilder sqlBuilder)
        {
            sqlBuilder.Append("{0}{1}{0}", this.quote, table);
        }

        public void AddClosingFromQuery(string alias, ISqlBuilder sqlBuilder)
        {
            sqlBuilder.PrependIndent();
            sqlBuilder.Append("{0} as {1}{2}{1}", Constants.ClosingParen, this.quote, alias);
        }

        public void AddOrderBy(OrderBy orderBy, ISqlBuilder sqlBuilder)
        {
            var alias = string.Empty;
            var formatStr = "{0}{2}{0} {3}";
            var order = orderBy.Order == common.SortOrder.Ascending ? Constants.Ascending : Constants.Descending;
            if (!string.IsNullOrWhiteSpace(orderBy.Source.Alias))
            {
                alias = orderBy.Source.Alias;
                formatStr = "{0}{1}{0}." + formatStr;
            }
            else
            {
                formatStr = "{1}" + formatStr;
            }

            sqlBuilder.Append(formatStr, this.quote, alias, orderBy.Name, order);
        }

        public void AddGroupBy(GroupBy groupBy, ISqlBuilder sqlBuilder)
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

            sqlBuilder.Append(formatStr, this.quote, alias, groupBy.Name);
        }
	}
}

