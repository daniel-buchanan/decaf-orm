using System;
using pdq.core.common;

namespace pdq.core.state.Conditionals.ValueFunctions
{
	public class DatePart : ValueFunction<int>
	{
		private DatePart(common.DatePart part)
			: base(ValueFunction.DatePart, part)
		{
		}

		public static DatePart Create(common.DatePart part) => new DatePart(part);
	}
}

