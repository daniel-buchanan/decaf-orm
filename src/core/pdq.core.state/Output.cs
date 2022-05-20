namespace pdq.core.state
{
	public class Output
	{
		private Output(string name, OutputSource source)
		{
			Name = name;
			Source = source;
		}

		public string Name { get; }

		public OutputSource Source { get; }

		public static Output Create(string name, OutputSource source)
        {
			return new Output(name, source);
        }

		public static Output Create(Column column, OutputSource source)
        {
			return new Output(column.Name, source);
        }
	}
}

