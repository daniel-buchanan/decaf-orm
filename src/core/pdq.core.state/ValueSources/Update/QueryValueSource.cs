namespace pdq.core.state.ValueSources.Update
{
	public class QueryValueSource : IUpdateValueSource
	{
		private QueryValueSource(state.Column column, IUpdateSource source)
		{
			Column = column;
			Source = source;
		}

		public IUpdateSource Source { get; private set; }

		public state.Column Column { get; private set; }

		public static QueryValueSource Create(state.Column column, IUpdateSource source)
        {
			return new QueryValueSource(column, source);
        }
	}
}

