using System;
using System.Collections.Generic;
using decaf.common.Utilities.Reflection;
using decaf.db.common;

namespace decaf.sqlserver
{
    public class SqlServerValueParser : ValueParser
    {
        public SqlServerValueParser(IReflectionHelper reflectionHelper)
            : base(reflectionHelper)
        {
            Replacements = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("'", "''"),
                new Tuple<string, string>("%%", ""),
                new Tuple<string, string>("--", "")
            };
        }

        protected override List<Tuple<string, string>> Replacements { get; }

        /// <inheritdoc/>
        public override bool ValueNeedsQuoting(Type type)
        {
            var underlyingType = reflectionHelper.GetUnderlyingType(type);

            if (underlyingType == typeof(bool)) return false;
            else if (underlyingType == typeof(byte[])) return false;
            else if (underlyingType == typeof(DateTime)) return true;
            else if (underlyingType == typeof(int)) return false;
            else if (underlyingType == typeof(double) ||
                        underlyingType == typeof(Single) ||
                        underlyingType == typeof(float) ||
                        underlyingType == typeof(decimal))
                return false;
            else if (underlyingType == typeof(string)) return true;
            else return true;
        }

        protected override bool BooleanFromString(string input)
            => input == "1";

        protected override string BooleanToString(bool input)
            => input ? "1" : "0";

        protected override byte[] BytesFromString(string input)
        {
            if (!input.StartsWith("0x")) return new byte[0];

            input = input.Substring(2);

            int NumberChars = input.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(input.Substring(i, 2), 16);
            return bytes;
        }

        protected override string BytesToString(byte[] input)
        {
            var hex = BitConverter.ToString(input);
            return "0x" + hex.Replace("-", "");
        }
    }
}

