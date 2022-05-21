using pdq.core.common;

namespace pdq.core.state.ValueFunctions
{
	public class ToUpper : ValueFunction<string>
	{
		private ToUpper()
			: base(ValueFunction.ToUpper)
		{
		}

		public static ToUpper Create() => new ToUpper();
	}
}

