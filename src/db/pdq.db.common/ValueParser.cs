using System;
using pdq.common.Utilities.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pdq.db.common
{
	public abstract class ValueParser : IValueParser
	{
        protected readonly IReflectionHelper reflectionHelper;

        public ValueParser(IReflectionHelper reflectionHelper)
		{
            this.reflectionHelper = reflectionHelper;
		}

        protected abstract List<Tuple<string, string>> Replacements { get; }

        /// <inheritdoc/>
        public T FromString<T>(string value)
        {
            var isNullable = this.reflectionHelper.IsNullableType<T>();
            var underlyingType = this.reflectionHelper.GetUnderlyingType<T>();

            if (isNullable && value == null) return default(T);
            if (string.IsNullOrWhiteSpace(value)) return default(T);

            if (underlyingType == typeof(byte[]))
                return ChangeType<T>(BytesFromString(value));

            if (underlyingType == typeof(bool))
                return ChangeType<T>(BooleanFromString(value));

            if (underlyingType == typeof(DateTime))
                return ChangeType<T>(Convert.ToDateTime(value));

            if (underlyingType == typeof(int))
                return ChangeType<T>(Convert.ToInt32(value));

            if (underlyingType == typeof(double) ||
                underlyingType == typeof(Single) ||
                underlyingType == typeof(float))
                return ChangeType<T>(Convert.ToDecimal(value));

            if (underlyingType == typeof(string))
            {
                var s = Convert.ToString(value);
                if (string.IsNullOrWhiteSpace(s)) return default(T);
                Replacements.ForEach(t => s = s.Replace(t.Item1, t.Item2));

                return ChangeType<T>(s);
            }

            return ChangeType<T>(Convert.ToString(value));
        }

        private T ChangeType<T>(object input)
            => (T)Convert.ChangeType(input, typeof(T));

        protected abstract byte[] BytesFromString(string input);

        protected abstract bool BooleanFromString(string input);

        /// <inheritdoc/>
        public string ToString<T>(T value) => ToString(value, value?.GetType() ?? typeof(T));

        /// <inheritdoc/>
        public string ToString(object value, Type type)
        {
            // get underlying type
            var underlyingType = this.reflectionHelper.GetUnderlyingType(type);

            // check for null and return
            if (value == null) return string.Empty;

            // check for byte array
            if (underlyingType == typeof(byte[]))
                return BytesToString(value as byte[]);

            // check for boolean
            if (underlyingType == typeof(bool))
            {
                var val = Convert.ToBoolean(value);
                return BooleanToString(val);
            }

            // check for date time
            if (underlyingType == typeof(DateTime))
            {
                // convert to db format
                return Convert.ToDateTime(value).ToString("yyyy-MM-ddTHH:mm:ss");
            }

            // check for integer
            if (underlyingType == typeof(int))
            {
                // convert to integer
                return Convert.ToInt32(value).ToString();
            }

            // check for double and float
            if (underlyingType == typeof(double) || underlyingType == typeof(float) || underlyingType == typeof(decimal))
            {
                // convert to decimal
                return Convert.ToDecimal(value).ToString();
            }

            // check for string
            if (underlyingType == typeof(string))
            {
                // convert to string
                var s = Convert.ToString(value);

                // replace any necessary fragments
                return Replacements.Aggregate(s, (current, r) => current.Replace(r.Item1, r.Item2));
            }

            // if type is enum
            if (underlyingType.IsEnum)
            {
                // return value as string
                return value.ToString();
            }

            // default to string
            return Convert.ToString(value);
        }

        protected abstract string BytesToString(byte[] input);

        protected abstract string BooleanToString(bool input);

        /// <inheritdoc/>
        public abstract bool ValueNeedsQuoting(Type type);

        /// <inheritdoc/>
        public bool ValueNeedsQuoting<T>(T value)
            => ValueNeedsQuoting(typeof(T));
    }
}

