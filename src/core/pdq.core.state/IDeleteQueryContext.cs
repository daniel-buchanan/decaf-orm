using pdq.common;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("pdq")]
namespace pdq.state
{
    public interface IDeleteQueryContext : IQueryContext
    {
		ITableTarget Table { get; }

		IWhere WhereClause { get; }

		IDeleteQueryContext From(ITableTarget target);

		IDeleteQueryContext Where(IWhere where);
	}
}

