using System;
namespace pdq.core
{
	public interface IOrderBy 
	{
		IOrderByThen OrderBy(string column, string tableAlias, OrderBy orderBy);
	}

	public interface IOrderByThen
    {
		IOrderByThen ThenBy(string column, string tableAlias, OrderBy orderBy);
	}
}

