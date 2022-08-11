using System;
namespace pdq.state.Utilities
{
    public static class DefaultValueHelper
    {
        /// <summary>
        /// Get the defult value for type known at compile time.
        /// </summary>
        /// <typeparam name="T">The type to get the default value for.</typeparam>
        /// <returns>The default value for <see cref="T"/>.</returns>
        public static T Get<T>() => (T)Get(typeof(T));

        /// <summary>
        /// Get the default value for a type known at runtime.
        /// </summary>
        /// <param name="type">The type to get the default value for.</param>
        /// <returns>The default value for the provided type.</returns>
        public static object Get(Type type)
        {
            if (type == null) return null;

            if (type.IsValueType)
                return Activator.CreateInstance(type);

            if (type == typeof(DateTime))
                return DateTime.MinValue;

            return null;
        }
    }
}

