using pdq.common;

namespace pdq.common.ValueFunctions
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

