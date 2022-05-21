using System;

namespace pdq.core.common
{
    public interface IQueryContext
	{
		Guid Id { get; }

		QueryType Kind { get; }
	}
}

