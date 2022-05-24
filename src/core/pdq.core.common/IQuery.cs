using System;

namespace pdq.core.common
{
	public interface IQuery
	{
		Guid Id { get; }

		QueryStatus Status { get; }
	}
}

