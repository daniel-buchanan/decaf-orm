using pdq.core.common;

namespace pdq.core
{
	public interface IOrderBy 
	{
		IOrderByThen OrderBy(string column, string tableAlias, SortOrder orderBy);
	}

	public interface IOrderByThen
    {
		IOrderByThen ThenBy(string column, string tableAlias, SortOrder orderBy);
	}
}

