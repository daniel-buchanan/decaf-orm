using System;
using pdq.common;

namespace pdq.state.Conditionals.ValueFunctions
{
	public abstract class ValueFunction<T> : Where, IValueFunction<T>
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

