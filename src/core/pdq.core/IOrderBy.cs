using pdq.common;

namespace pdq
{
	public interface IOrderBy :
		IExecute,
		ISelectColumn
	{
		/// <summary>
		/// Order by a given column.
		/// </summary>
		/// <param name="column">The column to order by.</param>
		/// <param name="tableAlias">The alias of the table the column belongs to.</param>
		/// <param name="orderBy">The direction of the order by, <see cref="SortOrder"/></param>
		/// <returns>(FluentAPI) The ability to continue order or other actions.</returns>
		IOrderByThen OrderBy(string column, string tableAlias, SortOrder orderBy);
	}

	public interface IOrderByThen :
		IExecute,
		ISelectColumn
	{
		IOrderByThen ThenBy(string column, string tableAlias, SortOrder orderBy);
	}
}

