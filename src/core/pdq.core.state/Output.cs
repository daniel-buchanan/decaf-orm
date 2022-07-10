namespace pdq.state
{
	public class Output
	{
		private Output(string name, OutputSources source)
		{
			Name = name;
			Source = source;
		}

		public string Name { get; }

		public OutputSources Source { get; }

		public static Output Create(string name, OutputSources source)
        {
			return new Output(name, source);
        }

		public static Output Create(Column column, OutputSources source)
        {
			return new Output(column.Name, source);
        }
	}
}

