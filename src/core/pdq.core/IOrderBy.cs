using System;
namespace pdq.core
{
	public interface IOrderBy : IBuilder<IOrderBy>
	{
		IOrderByThen OrderBy(string column, string tableAlias, OrderBy orderBy);
	}

	public interface IOrderByThen : IBuilder<IOrderByThen>
    {
		IOrderByThen ThenBy(string column, string tableAlias, OrderBy orderBy);
	}
}

