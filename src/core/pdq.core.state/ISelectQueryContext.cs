using System.Collections.Generic;
using System.Linq;
using pdq.core.common;

namespace pdq.core.state
{
    public interface ISelectQueryContext : IQueryContext
    {
		IReadOnlyCollection<Table> Tables { get; }

		IReadOnlyCollection<Column> Columns { get; }

		IReadOnlyCollection<Join> Joins { get; }

		IWhere WhereClause { get; }

		IOrderedEnumerable<OrderBy> OrderBy { get; }

		IOrderedEnumerable<GroupBy> GroupBy { get; }
	}
}

