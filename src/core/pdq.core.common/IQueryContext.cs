using System;
using System.Collections.Generic;

namespace pdq.common
{
    public interface IQueryContext : IDisposable
	{
		Guid Id { get; }

		QueryType Kind { get; }

		IEnumerable<IQueryTarget> QueryTargets { get; }
	}
}

