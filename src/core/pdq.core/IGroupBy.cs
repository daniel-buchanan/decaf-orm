using pdq.common;

namespace pdq
{
	public interface IGroupBy 
	{
		IGroupByThen GroupBy(string column, string tableAlias);
	}

	public interface IGroupByThen
    {
		IOrderByThen ThenBy(string column, string tableAlias);
	}
}

