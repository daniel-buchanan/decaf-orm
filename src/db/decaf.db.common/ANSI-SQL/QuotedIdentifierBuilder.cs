using decaf.common;
using decaf.db.common.Builders;
using decaf.state;

namespace decaf.db.common.ANSISQL;

public class QuotedIdentifierBuilder : IQuotedIdentifierBuilder
{
    private const string QuotedFormat = "{0}{1}{0}";
    private const string QuotedFormatDot = "{0}{1}{0}.";
    protected readonly string Quote = string.Empty;
    protected readonly IConstants Constants;

    protected QuotedIdentifierBuilder(
        IDatabaseOptions options,
        IConstants constants)
    {
        this.Constants = constants;
        if (options.QuotedIdentifiers)
            Quote = constants.ColumnQuote;
    }

    public virtual void AddSelect(Column column, ISqlBuilder sqlBuilder)
    {
        if (!string.IsNullOrWhiteSpace(column.Source?.Alias))
            sqlBuilder.Append(QuotedFormatDot, Quote, column.Source.Alias);

        if (string.IsNullOrWhiteSpace(column.NewName))
            sqlBuilder.Append(QuotedFormat, Quote, column.Name);
        else
            sqlBuilder.Append("{0}{1}{0} as {0}{2}{0}", Quote, column.Name, column.NewName);
    }

    public virtual void AddColumn(Column column, ISqlBuilder sqlBuilder)
    {
        if (!string.IsNullOrWhiteSpace(column.Source?.Alias))
            sqlBuilder.Append(QuotedFormatDot, Quote, column.Source.Alias);

        sqlBuilder.Append(QuotedFormat, Quote, column.Name);
    }

    public virtual void AddOutput(Output output, ISqlBuilder sqlBuilder)
        => sqlBuilder.Append(QuotedFormat, Quote, output.Name);

    public virtual void AddFromTable(ITableTarget tableTarget, ISqlBuilder sqlBuilder)
    {
        var format = string.Empty;

        if (!string.IsNullOrWhiteSpace(tableTarget.Schema))
            format = QuotedFormatDot;

        if (!string.IsNullOrWhiteSpace(tableTarget.Alias))
            format += "{0}{2}{0} as {0}{3}{0}";

        sqlBuilder.Append(format, Quote, tableTarget.Schema, tableTarget.Name, tableTarget.Alias);
    }

    public virtual void AddFromTable(string table, ISqlBuilder sqlBuilder)
    {
        sqlBuilder.Append(QuotedFormat, Quote, table);
    }

    public virtual void AddClosingFromQuery(string alias, ISqlBuilder sqlBuilder)
    {
        sqlBuilder.PrependIndent();
        sqlBuilder.Append("{0} as {1}{2}{1}", Constants.ClosingParen, Quote, alias);
    }

    public virtual void AddOrderBy(OrderBy orderBy, ISqlBuilder sqlBuilder)
    {
        var alias = string.Empty;
        var formatStr = "{0}{2}{0} {3}";
        var order = orderBy.Order == SortOrder.Ascending ? Constants.Ascending : Constants.Descending;
        if (!string.IsNullOrWhiteSpace(orderBy.Source.Alias))
        {
            alias = orderBy.Source.Alias;
            formatStr = QuotedFormatDot + formatStr;
        }
        else
        {
            formatStr = "{1}" + formatStr;
        }

        sqlBuilder.Append(formatStr, Quote, alias, orderBy.Name, order);
    }

    public virtual void AddGroupBy(GroupBy groupBy, ISqlBuilder sqlBuilder)
    {
        var alias = string.Empty;
        var formatStr = "{0}{2}{0} {3}";
        if (!string.IsNullOrWhiteSpace(groupBy.Source.Alias))
        {
            alias = groupBy.Source.Alias;
            formatStr = QuotedFormatDot + formatStr;
        }
        else
        {
            formatStr = "{1}" + formatStr;
        }

        sqlBuilder.Append(formatStr, Quote, alias, groupBy.Name);
    }
}