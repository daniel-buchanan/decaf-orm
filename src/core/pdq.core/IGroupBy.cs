using pdq.common;

namespace pdq
{
	public interface IGroupBy : IOrderBy, ISelectColumn
	{
		IGroupByThen GroupBy(string column, string tableAlias);
	}

	public interface IGroupByThen : IOrderBy, ISelectColumn
    {
		IGroupByThen ThenBy(string column, string tableAlias);
	}
}

