namespace pdq.core.state
{
	public interface IQueryContext
	{
		Guid Id { get; }

		ContextKind Kind { get; }
	}

	public interface ISelectQueryContext : IQueryContext
    {
		IReadOnlyCollection<Table> Tables { get; }

		IReadOnlyCollection<Column> Columns { get; }

		IReadOnlyCollection<Join> Joins { get; }

		IWhere WhereClause { get; }

		IOrderedEnumerable<OrderBy> OrderBy { get; }

		IOrderedEnumerable<GroupBy> GroupBy { get; }
	}

	public interface IInsertQueryContext : IQueryContext
    {
		Table Table { get; }

		IReadOnlyCollection<Column> Columns { get; }

		IInsertSource Source { get; }

		IReadOnlyCollection<Output> Outputs { get; }
	}

	public interface IUpdateQueryContext : IQueryContext
    {
		Table Table { get; }

		IReadOnlyCollection<Tuple<Column, IUpdateValueSource>> Updates { get; }

		IUpdateSource Source { get; }

		IWhere WhereClause { get; }

		IReadOnlyCollection<Output> Outputs { get; }
    }

	public interface IDeleteQueryContext : IQueryContext
    {
		Table Table { get; }

		IWhere WhereClause { get; }
	}
}

