using System;
using pdq.common;

namespace pdq.state.Conditionals.ValueFunctions
{
	public class StringContains : ValueFunction<string>
	{
		private StringContains()
			: base(ValueFunction.Contains)
		{
		}

		public static StringContains Create() => new StringContains();
	}
}

