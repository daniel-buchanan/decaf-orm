using System;

namespace pdq.common
{
	public interface IQuery : IDisposable
	{
		Guid Id { get; }

		QueryStatus Status { get; }
	}
}

