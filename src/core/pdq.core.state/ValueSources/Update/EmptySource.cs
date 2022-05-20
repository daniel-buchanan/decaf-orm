namespace pdq.core.state.ValueSources.Update
{
	public class EmptySource : IUpdateSource
	{
		private EmptySource() { }

		public static EmptySource Create() => new EmptySource();
	}
}

