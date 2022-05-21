using System;

namespace pdq.core.common
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

