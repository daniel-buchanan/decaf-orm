using System;
using pdq.common;

namespace pdq.state
{
	public interface ISelectQueryTarget : IQueryTarget
	{
        ISelectQueryContext Context { get; }
    }
}

