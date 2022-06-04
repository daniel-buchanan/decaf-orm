using pdq.common;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("pdq")]
namespace pdq.state
{
    public interface IDeleteQueryContext : IQueryContext
    {
		Table Table { get; }

		IWhere WhereClause { get; }

		IDeleteQueryContext From(Table table);

		IDeleteQueryContext Where(IWhere where);
	}
}

