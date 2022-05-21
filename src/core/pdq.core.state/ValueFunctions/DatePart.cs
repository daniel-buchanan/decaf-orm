using System;
using pdq.core.common;

namespace pdq.core.state.ValueFunctions
{
	public class DatePart : ValueFunction<DateTime>
	{
		private DatePart(common.DatePart part)
			: base(ValueFunction.DatePart, part)
		{
		}

		public static DatePart Create(common.DatePart part) => new DatePart(part);
	}
}

