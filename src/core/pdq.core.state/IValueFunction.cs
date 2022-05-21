using System;
using pdq.core.common;

namespace pdq.core.state
{
	public interface IValueFunction
    {
		ValueFunction Type { get; }

		object[] Arguments { get; }

		Type ValueType { get; }
	}

	public interface IValueFunction<T> : IValueFunction
	{
		
	}
}

