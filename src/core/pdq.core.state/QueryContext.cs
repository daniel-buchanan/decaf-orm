using System;
using System.Collections.Generic;
using pdq.common;

namespace pdq.state
{
	public abstract class QueryContext : IQueryContext
	{
		protected readonly List<IQueryTarget> queryTargets;

		protected QueryContext(QueryType kind)
		{
			Id = Guid.NewGuid();
			Kind = kind;
			this.queryTargets = new List<IQueryTarget>();
		}

        public Guid Id { get; private set; }

        public QueryType Kind { get; private set; }

        public IEnumerable<IQueryTarget> QueryTargets => this.queryTargets;

        public abstract void Dispose();
    }
}

