using pdq.common;

namespace pdq.state.Conditionals.ValueFunctions
{
	public class ToLower : ValueFunction<string>
	{
		private ToLower()
			: base(ValueFunction.ToLower)
		{
		}

		public static ToLower Create() => new ToLower();
	}
}

