namespace pdq.state.Conditionals
{
	public class Or : Where
	{
		private Or(params IWhere[] children) : base(children) { }

		public static Or Where(params IWhere[] items)
        {
			return new Or(items);
        }
	}
}

