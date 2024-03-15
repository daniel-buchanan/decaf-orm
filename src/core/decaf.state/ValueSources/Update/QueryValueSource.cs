using decaf.common;

namespace decaf.state.ValueSources.Update
{
	public class QueryValueSource : IUpdateValueSource
	{
		private QueryValueSource(Column destinationColumn, Column sourceColumn, IQueryTarget source)
		{
			DestinationColumn = destinationColumn;
			SourceColumn = sourceColumn;
			Source = source;
		}

		public IQueryTarget Source { get; private set; }

		public Column SourceColumn { get; private set; }

        public Column DestinationColumn { get; private set; }

        public static QueryValueSource Create(Column destinationColumn, Column sourceColumn, IQueryTarget source)
			=> new QueryValueSource(destinationColumn, sourceColumn, source);
    }
}

