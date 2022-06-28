using System;
using pdq.common;

namespace pdq.state.Conditionals.ValueFunctions
{
	public class StringStartsWith : ValueFunction<string>
	{
		private StringStartsWith()
			: base(ValueFunction.StartsWith)
		{
		}

		public static StringStartsWith Create() => new StringStartsWith();
	}
}

