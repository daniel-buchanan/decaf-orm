using System;
using pdq.common;

namespace pdq.state.Conditionals.ValueFunctions
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

