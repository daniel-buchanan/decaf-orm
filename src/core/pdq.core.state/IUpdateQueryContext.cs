using System;
using System.Collections.Generic;
using pdq.common;

namespace pdq.state
{
    public interface IUpdateQueryContext : IQueryContext
    {
		ITableTarget Table { get; }

		IReadOnlyCollection<IUpdateValueSource> Updates { get; }

		IQueryTarget Source { get; }

		IWhere WhereClause { get; }

		IReadOnlyCollection<Output> Outputs { get; }

		void Update(ITableTarget target);

		void From(IQueryTarget source);

		void Where(IWhere where);

		void Output(Output output);

		void Set(IUpdateValueSource value);
    }
}

