using System;
using System.Collections.Generic;
using decaf.common.Utilities.Reflection;
using decaf.db.common;
using decaf.db.common.Builders;

namespace decaf.sqlserver
{
    public class SqlServerValueParser(IReflectionHelper reflectionHelper, IConstants constants)
        : ValueParser(reflectionHelper, constants)
    {
        protected override List<Tuple<string, string>> Replacements { get; } = new()
        {
            new Tuple<string, string>("'", "''"),
            new Tuple<string, string>("%%", ""),
            new Tuple<string, string>("--", "")
        };

        /// <inheritdoc/>
        public override bool ValueNeedsQuoting(Type type)
        {
            var underlyingType = ReflectionHelper.GetUnderlyingType(type);

            if (underlyingType == typeof(bool)) return false;
            if (underlyingType == typeof(byte[])) return false;
            if (underlyingType == typeof(DateTime)) return true;
            if (underlyingType == typeof(int)) return false;
            if (underlyingType == typeof(double) ||
                underlyingType == typeof(float) ||
                underlyingType == typeof(decimal))
                return false;
            return underlyingType == typeof(string);
        }

        protected override byte[] BytesFromString(string input)
        {
            if (!input.StartsWith("0x")) return [];

            input = input.Substring(2);

            int numberChars = input.Length;
            var bytes = new byte[numberChars / 2];
            for (var i = 0; i < numberChars; i += 2)
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

