using System;
using pdq.common;

namespace pdq.state
{
	public abstract class QueryContext : IQueryContext
	{
		protected QueryContext(QueryType kind)
		{
			Id = Guid.NewGuid();
			Kind = kind;
		}

        public Guid Id { get; private set; }

        public QueryType Kind { get; private set; }

		public abstract void Dispose();
    }
}

