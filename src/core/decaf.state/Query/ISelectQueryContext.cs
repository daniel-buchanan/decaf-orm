using System.Collections.Generic;
using decaf.common;

namespace decaf.state
{
    public interface ISelectQueryContext : IQueryContext
    {
		/// <summary>
        /// The set of <see cref="Column"/> for this select query.
        /// </summary>
		IReadOnlyCollection<Column> Columns { get; }

        /// <summary>
        /// The set of <see cref="Join"/> for this select query.
        /// </summary>
        IReadOnlyCollection<Join> Joins { get; }

        /// <summary>
        /// Any <see cref="IWhere"/> clauses to filter this select query.
        /// </summary>
        IWhere WhereClause { get; }

		/// <summary>
        /// The set of <see cref="state.OrderBy"/> clauses for this select query.
        /// </summary>
		IReadOnlyCollection<OrderBy> OrderByClauses { get; }

		/// <summary>
        /// The set of <see cref="state.GroupBy"/> clauses for this select query.
        /// </summary>
		IReadOnlyCollection<GroupBy> GroupByClauses { get; }

        /// <summary>
        /// The number of rows to return.
        /// </summary>
        int? RowLimit { get; }

        /// <summary>
        /// The <see cref="IQueryTarget"/> to select from.
        /// </summary>
        /// <param name="table">The <see cref="IQueryTarget"/> to use.</param>
        void From(IQueryTarget table);

        /// <summary>
        /// Select a single <see cref="Column"/>.
        /// </summary>
        /// <param name="column">The <see cref="Column"/> to select and add to this query.</param>
        void Select(Column column);

        /// <summary>
        /// Add a join to this query.
        /// </summary>
        /// <param name="join">The <see cref="state.Join"/> to add.</param>
		void Join(Join join);

        /// <summary>
        /// Order the query by a given column.
        /// </summary>
        /// <param name="orderBy">The <see cref="state.OrderBy"/> to add.</param>
		void OrderBy(OrderBy orderBy);

        /// <summary>
        /// Group the query by a given column.
        /// </summary>
        /// <param name="groupBy">The <see cref="state.GroupBy"/> to add.</param>
		void GroupBy(GroupBy groupBy);

        /// <summary>
        /// Add a where clause to this select query.
        /// </summary>
        /// <param name="where">The <see cref="IWhere"/> to add.</param>
		void Where(IWhere where);

        /// <summary>
        /// Add a row limit on the results.
        /// </summary>
        /// <param name="limit">The number of rows to return.</param>
        void Limit(int limit);
	}
}

