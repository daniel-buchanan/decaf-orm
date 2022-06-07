using pdq.common;

namespace pdq
{
	public interface IOrderBy : IExecute
	{
		IOrderByThen OrderBy(string column, string tableAlias, SortOrder orderBy);
	}

	public interface IOrderByThen : IExecute
    {
		IOrderByThen ThenBy(string column, string tableAlias, SortOrder orderBy);
	}
}

