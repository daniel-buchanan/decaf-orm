using System;
using pdq.common;

namespace pdq.state
{
	public interface ITableTarget : IQueryTarget
    {
		string Name { get; }

		string Schema { get; }

		bool IsEquivalentTo(ITableTarget target);
    }
}

