using System;
using System.Collections.Generic;
using pdq.common;

namespace pdq.state
{
    public interface IUpdateQueryContext : IQueryContext
    {
		ITableTarget Table { get; }

		IReadOnlyCollection<Tuple<Column, IUpdateValueSource>> Updates { get; }

		IUpdateSource Source { get; }

		IWhere WhereClause { get; }

		IReadOnlyCollection<Output> Outputs { get; }
    }
}

