using decaf.common;

namespace decaf.state.Conditionals
{
	public class Not : Where
	{
		private Not(IWhere item)
		{
			Item = item;
		}

		public IWhere Item { get; private set; }

		public static IWhere This(IWhere item) => new Not(item);
	}
}

