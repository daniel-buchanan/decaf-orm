using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using pdq.db.common;
using pdq.state.Utilities;

namespace pdq.npgsql
{
    public class NpgsqlValueParser : IValueParser
    {
        private readonly IReflectionHelper reflectionHelper;
        private readonly List<Tuple<string, string>> replacements;

        public NpgsqlValueParser(IReflectionHelper reflectionHelper)
        {
            this.reflectionHelper = reflectionHelper;
            this.replacements = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("'", "''"),
                new Tuple<string, string>("%%", ""),
                new Tuple<string, string>("--", "")
            };
        }

        /// <inheritdoc/>
        public T FromString<T>(string value)
        {
            bool isNullable = this.reflectionHelper.IsNullableType<T>();
            Type underlyingType = this.reflectionHelper.GetUnderlyingType<T>();

            if (isNullable && value == null) return default(T);

            object toReturn;

            if (underlyingType == typeof(byte[]))
            {
                if (value.StartsWith(@"\x") || value.StartsWith(@"\\x"))
                {
                    if (value.StartsWith("@\\x")) value = value.Substring(3);
                    else if (value.StartsWith(@"\x")) value = value.Substring(2);

                    int NumberChars = value.Length / 2;
                    byte[] bytes = new byte[NumberChars];
                    using (var sr = new StringReader(value))
                    {
                        for (int i = 0; i < NumberChars; i++)
                            bytes[i] = Convert.ToByte(new string(new char[2] { (char)sr.Read(), (char)sr.Read() }), 16);
                    }
                    toReturn = bytes;
                }
                else
                {
                    toReturn = null;
                }
            }
            else if (underlyingType == typeof(bool))
            {
                toReturn = value == "1" ? true : false;
            }
            else if (underlyingType == typeof(DateTime))
            {
                toReturn = Convert.ToDateTime(value);
            }
            else if (underlyingType == typeof(int))
            {
                toReturn = Convert.ToInt32(value);
            }
            else if (underlyingType == typeof(double) || underlyingType == typeof(Single) || underlyingType == typeof(float))
            {
                toReturn = Convert.ToDecimal(value);
            }
            else if (underlyingType == typeof(string))
            {
                string s = Convert.ToString(value);
                if (s == null)
                {
                    return default(T);
                }

                foreach (Tuple<string, string> r in this.replacements)
                {
                    s = s.Replace(r.Item1, r.Item2);
                }

                toReturn = s;
            }
            else
            {
                if (value == null)
                {
                    return default(T);
                }

                toReturn = Convert.ToString(value);
            }

            return (T)Convert.ChangeType(toReturn, typeof(T));
        }

        /// <inheritdoc/>
        public string QuoteValue<T>(T value) => QuoteValue(value, typeof(T));

        /// <inheritdoc/>
        public string QuoteValue(object value, Type type)
        {
            var str = ToString(value, type);

            if (ValueNeedsQuoting(type))
                return $"'{str}'";
            return str;
        }

        /// <inheritdoc/>
        public string ToString<T>(T value) => ToString(value, value?.GetType() ?? typeof(T));

        /// <inheritdoc/>
        public string ToString(object value, Type type)
        {
            // get underlying type
            var underlyingType = this.reflectionHelper.GetUnderlyingType(type);

            // check for null and return
            if (value == null) return null;

            // check for byte array
            if (underlyingType == typeof(byte[]))
            {
                // get byte array as string
                string valueString = BitConverter.ToString(value as byte[]).Replace("-", "");

                // return as string
                return $@"\x{valueString}";
            }

            // check for boolean
            if (underlyingType == typeof(bool))
            {
                // convert value to boolean
                bool val = Convert.ToBoolean(value);

                // return as zero or one
                return val ? "1" : "0";
            }

            // check for date time
            if (underlyingType == typeof(DateTime))
            {
                // convert to db format
                return Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss");
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
                return this.replacements.Aggregate(s, (current, r) => current.Replace(r.Item1, r.Item2));
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

        /// <inheritdoc/>
        public bool ValueNeedsQuoting<T>(T value) => ValueNeedsQuoting(typeof(T));

        /// <inheritdoc/>
        public bool ValueNeedsQuoting(Type type)
        {
            bool isNullable = this.reflectionHelper.IsNullableType(type);
            Type underlyingType = this.reflectionHelper.GetUnderlyingType(type);

            if (underlyingType == typeof(bool))
            {
                return true;
            }
            else if (underlyingType == typeof(byte[]))
            {
                return true;
            }
            else if (underlyingType == typeof(DateTime))
            {
                return true;
            }
            else if (underlyingType == typeof(int))
            {
                return false;
            }
            else if (underlyingType == typeof(double) ||
                        underlyingType == typeof(Single) ||
                        underlyingType == typeof(float) ||
                        underlyingType == typeof(decimal))
            {
                return true;
            }
            else if (underlyingType == typeof(string))
            {
                return true;
            }
            else
            {
                return true;
            }
        }
    }
}

