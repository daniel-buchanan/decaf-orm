using pdq.core.common;

namespace pdq.core.state.ValueFunctions
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

