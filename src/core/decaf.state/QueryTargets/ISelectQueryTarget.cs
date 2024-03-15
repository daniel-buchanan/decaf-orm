using System;
using decaf.common;

namespace decaf.state
{
	public interface ISelectQueryTarget : IQueryTarget
	{
        ISelectQueryContext Context { get; }
    }
}

