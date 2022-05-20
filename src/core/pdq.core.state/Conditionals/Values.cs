namespace pdq.core.state.Conditionals
{
	public abstract class Values : Where
	{
		public abstract Type ValueType { get; protected set; }

		public static Values<T> In<T>(state.Column column, params T[] values)
        {
			return new Values<T>(column, values);
        }
	}

	public class Values<T> : Values
    {
		internal Values(state.Column column, params T[] values)
        {
			ValueType = typeof(T);
			Column = column;
			ValueSet = values?.ToList().AsReadOnly() ?? new List<T>().AsReadOnly();
        }

		public state.Column Column { get; private set; }

		public IReadOnlyCollection<T> ValueSet { get; private set; }

		public override Type ValueType { get; protected set; }
	}
}

