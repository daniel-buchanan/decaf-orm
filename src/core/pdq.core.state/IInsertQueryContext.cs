using System.Collections.Generic;
using pdq.core.common;

namespace pdq.core.state
{
    public interface IInsertQueryContext : IQueryContext
    {
		Table Table { get; }

		IReadOnlyCollection<Column> Columns { get; }

		IInsertSource Source { get; }

		IReadOnlyCollection<Output> Outputs { get; }
	}
}

