using decaf.state;

namespace decaf.db.common.Builders;

public interface IQuotedIdentifierBuilder
{
    void AddClosingFromQuery(string alias, ISqlBuilder sqlBuilder);
    void AddColumn(Column column, ISqlBuilder sqlBuilder);
    void AddFromTable(ITableTarget tableTarget, ISqlBuilder sqlBuilder);
    void AddFromTable(string table, ISqlBuilder sqlBuilder);
    void AddGroupBy(GroupBy groupBy, ISqlBuilder sqlBuilder);
    void AddOrderBy(OrderBy orderBy, ISqlBuilder sqlBuilder);
    void AddOutput(Output output, ISqlBuilder sqlBuilder);
    void AddSelect(Column column, ISqlBuilder sqlBuilder);
}