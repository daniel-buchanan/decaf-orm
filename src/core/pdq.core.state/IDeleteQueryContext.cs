using pdq.core.common;

namespace pdq.core.state
{
    public interface IDeleteQueryContext : IQueryContext
    {
		Table Table { get; }

		IWhere WhereClause { get; }
	}
}

