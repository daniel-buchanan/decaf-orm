using System.Collections.Generic;
using pdq.common;

namespace pdq.state
{
    public interface IInsertQueryContext : IQueryContext
    {
		ITableTarget Target { get; }

		IReadOnlyCollection<Column> Columns { get; }

		IInsertValuesSource Source { get; }

		IReadOnlyCollection<Output> Outputs { get; }

		IInsertQueryContext Into(ITableTarget target);

		IInsertQueryContext Column(Column column);

		IInsertQueryContext From(IInsertValuesSource source);

		IInsertQueryContext Output(Output output);
	}
}

