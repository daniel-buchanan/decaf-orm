namespace pdq.core.state.ValueSources.update
{
	public abstract class StaticValueSource : IUpdateValueSource
	{
		public abstract Type ValueType { get; }

		public static StaticValueSource<T> Create<T>(state.Column column, T value)
        {
			return new StaticValueSource<T>(column, value);
        }
	}

	public class StaticValueSource<T> : StaticValueSource
	{
		internal StaticValueSource(state.Column column, T value)
		{
			Column = column;
			Value = value;
			ValueType = typeof(T);
		}

		public state.Column Column { get; private set; }

		public T Value { get; private set; }

        public override Type ValueType { get; }
	}
}

