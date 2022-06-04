using System;

namespace pdq.common
{
    public interface IQueryContext : IDisposable
	{
		Guid Id { get; }

		QueryType Kind { get; }
	}
}

