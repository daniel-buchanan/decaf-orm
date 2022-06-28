using System.Collections.Generic;
using System.Linq;
using pdq.common;

namespace pdq.state
{
    public interface ISelectQueryContext : IQueryContext
    {
		IReadOnlyCollection<Column> Columns { get; }

		IReadOnlyCollection<Join> Joins { get; }

		IWhere WhereClause { get; }

		IReadOnlyCollection<OrderBy> OrderByClauses { get; }

		IReadOnlyCollection<GroupBy> GroupByClauses { get; }

		ISelectQueryContext From(IQueryTarget table);

		ISelectQueryContext Select(Column column);

		ISelectQueryContext Join(Join join);

		ISelectQueryContext OrderBy(OrderBy orderBy);

		ISelectQueryContext GroupBy(GroupBy groupBy);

		ISelectQueryContext Where(IWhere where);
	}
}

