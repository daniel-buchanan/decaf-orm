using System;
using pdq.core.common;

namespace pdq.core.state.Conditionals.ValueFunctions
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

