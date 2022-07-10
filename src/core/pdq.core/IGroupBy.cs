using pdq.common;

namespace pdq
{
	public interface IGroupBy : IExecute
	{
		IGroupByThen GroupBy(string column, string tableAlias);
	}

	public interface IGroupByThen : IExecute
    {
		IGroupByThen ThenBy(string column, string tableAlias);
	}
}

