using System;
using pdq.common;

namespace pdq.state
{
	public interface IValueFunction : IWhere
    {
		ValueFunction Type { get; }

		object[] Arguments { get; }

		Type ValueType { get; }
	}
}

