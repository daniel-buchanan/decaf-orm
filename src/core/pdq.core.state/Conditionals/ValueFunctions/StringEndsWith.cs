using System;
using pdq.common;

namespace pdq.state.Conditionals.ValueFunctions
{
	public class StringEndsWith : ValueFunction<string>
	{
        private StringEndsWith(string value)
            : base(ValueFunction.EndsWith)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public static StringEndsWith Create(string value) => new StringEndsWith(value);
    }
}

