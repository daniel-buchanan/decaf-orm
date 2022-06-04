using System;

namespace pdq.common
{
	[Flags]
	public enum QueryType
	{
		None,
		Select,
		Update,
		Insert,
		Delete
	}
}

