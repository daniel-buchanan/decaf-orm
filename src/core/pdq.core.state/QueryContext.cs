namespace pdq.core.state
{
	public abstract class QueryContext : IQueryContext
	{
		public QueryContext()
		{
			Id = Guid.NewGuid();
		}

        public Guid Id { get; private set; }

        public abstract ContextKind Kind { get; }
    }
}

