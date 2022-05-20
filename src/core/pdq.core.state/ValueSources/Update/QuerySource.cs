namespace pdq.core.state.ValueSources.Update
{
	public class QuerySource : IUpdateSource
	{
		private QuerySource(ISelectQueryContext context)
		{
			Query = context;
		}

		public ISelectQueryContext Query { get; private set; }

		public static QuerySource Create(ISelectQueryContext context)
        {
			return new QuerySource(context);
        }
	}
}

