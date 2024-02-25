using System;
using decaf.common;

namespace decaf.common.ValueFunctions
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

