using System;

namespace pdq.common
{
	public interface IQuery
	{
		Guid Id { get; }

		QueryStatus Status { get; }
	}
}

