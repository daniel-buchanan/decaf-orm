using pdq.core.common;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("pdq.core")]
namespace pdq.core.state
{
    public interface IDeleteQueryContext : IQueryContext
    {
		Table? Table { get; }

		IWhere? WhereClause { get; }

		IDeleteQueryContext From(Table table);

		IDeleteQueryContext Where(IWhere where);
	}
}

