using System;

namespace pdq.db.common
{
    public interface IValueParser
    {
        /// <summary>
        /// Parse a string to a given type
        /// </summary>
        /// <typeparam name="T">The type to parse the string to</typeparam>
        /// <param name="value">The string value to parse</param>
        /// <returns>The converted value, or the default for the type specified.</returns>
        T FromString<T>(string value);

        /// <summary>
        /// Parses a value and performs replaces and corrections to ensure validity
        /// </summary>
        /// <param name="value">The value to parse</param>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <returns></returns>
        string ToString<T>(T value);

        /// <summary>
        /// Parses a value and performs replaces and corrections to ensure validity
        /// </summary>
        /// <param name="value">The value to parse</param>
        /// <param name="type">The type fo perform parsing/validation on</param>
        /// <returns></returns>
        string ToString(object value, Type type);

        /// <summary>
        /// Determines if a value needs to be quoted
        /// </summary>
        /// <param name="type">The type to use.</param>
        /// <returns>True if the value needs quoting, False if not.</returns>
        bool ValueNeedsQuoting(Type type);

        /// <summary>
        /// Determines if a value needs to be quoted
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value to check for quoting</param>
        /// <returns>True if the value needs quoting, False if not.</returns>
        bool ValueNeedsQuoting<T>(T value);
    }
}