using System;
namespace pdq.core.Where
{
	internal enum WhereItemType
	{
		And,
		Or,
		Between,
		In,
		ColumnMatch,
		ValueMatch,
		DatePart,
		NativeFunction
	}
}

