using System;
using pdq.core.common;

namespace pdq.core.state.ValueFunctions
{
	public abstract class ValueFunction<T> : IValueFunction<T>
	{
		protected ValueFunction(ValueFunction type, params object[] arguments)
		{
			Type = type;
			Arguments = arguments;
		}

        public ValueFunction Type { get; private set; }

        public object[] Arguments { get; private set; }

		public Type ValueType => typeof(T);
    }
}

