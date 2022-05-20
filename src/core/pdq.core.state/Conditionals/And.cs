namespace pdq.core.state.Conditionals
{
	public class And : Where
	{
		private And(params IWhere[] children) : base(children) { }

		public static And Where(params IWhere[] items)
        {
			return new And(items);
        }
	}
}

