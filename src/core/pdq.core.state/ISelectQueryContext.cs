using System.Collections.Generic;
using pdq.common;

namespace pdq.state
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
        /// The <see cref="IQueryTarget"/> to select from.
        /// </summary>
        /// <param name="table">The <see cref="IQueryTarget"/> to use.</param>
        /// <returns>This <see cref="ISelectQueryContext"/>.</returns>
        ISelectQueryContext From(IQueryTarget table);

        /// <summary>
        /// Select a single <see cref="Column"/>.
        /// </summary>
        /// <param name="column">The <see cref="Column"/> to select and add to this query.</param>
        /// <returns>This <see cref="ISelectQueryContext"/>.</returns>
        ISelectQueryContext Select(Column column);

        /// <summary>
        /// Add a join to this query.
        /// </summary>
        /// <param name="join">The <see cref="state.Join"/> to add.</param>
        /// <returns>This <see cref="ISelectQueryContext"/>.</returns>
		ISelectQueryContext Join(Join join);

        /// <summary>
        /// Order the query by a given column.
        /// </summary>
        /// <param name="orderBy">The <see cref="state.OrderBy"/> to add.</param>
        /// <returns>This <see cref="ISelectQueryContext"/>.</returns>
		ISelectQueryContext OrderBy(OrderBy orderBy);

        /// <summary>
        /// Group the query by a given column.
        /// </summary>
        /// <param name="groupBy">The <see cref="state.GroupBy"/> to add.</param>
        /// <returns>This <see cref="ISelectQueryContext"/>.</returns>
		ISelectQueryContext GroupBy(GroupBy groupBy);

        /// <summary>
        /// Add a where clause to this select query.
        /// </summary>
        /// <param name="where">The <see cref="IWhere"/> to add.</param>
        /// <returns>This <see cref="ISelectQueryContext"/>.</returns>
		ISelectQueryContext Where(IWhere where);
	}
}

