using System;

namespace pdq.common
{
    [Flags]
	public enum QueryType
	{
		None = 0,
		Select = 1,
		Update = 2,
		Insert = 4,
		Delete = 8
	}
}

