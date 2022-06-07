using pdq.common;

namespace pdq
{
	public interface IGroupBy : IExecute
	{
		IGroupByThen GroupBy(string column, string tableAlias);
	}

	public interface IGroupByThen : IExecute
    {
		IOrderByThen ThenBy(string column, string tableAlias);
	}
}

