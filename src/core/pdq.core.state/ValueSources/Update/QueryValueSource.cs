using pdq.common;

namespace pdq.state.ValueSources.Update
{
	public class QueryValueSource : IUpdateValueSource
	{
		private QueryValueSource(state.Column column, IQueryTarget source)
		{
			Column = column;
			Source = source;
		}

		public IQueryTarget Source { get; private set; }

		public state.Column Column { get; private set; }

		public static QueryValueSource Create(state.Column column, IQueryTarget source)
        {
			return new QueryValueSource(column, source);
        }
	}
}

