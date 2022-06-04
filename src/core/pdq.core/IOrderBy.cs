using pdq.common;

namespace pdq
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

