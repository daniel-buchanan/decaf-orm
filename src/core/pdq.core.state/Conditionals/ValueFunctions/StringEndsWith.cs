using System;
using pdq.common;

namespace pdq.state.Conditionals.ValueFunctions
{
	public class StringEndsWith : ValueFunction<string>
	{
		private StringEndsWith()
			: base(ValueFunction.EndsWith)
		{
		}

		public static StringEndsWith Create() => new StringEndsWith();
	}
}

