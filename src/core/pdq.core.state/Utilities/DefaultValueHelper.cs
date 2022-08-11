using System;
namespace pdq.state.Utilities
{
    public class DefaultValueHelper
    {
        public static T Get<T>() => default(T);

        public static object Get(Type type)
        {
            if (type.IsValueType)
                return Activator.CreateInstance(type);

            if (type == typeof(DateTime))
                return DateTime.MinValue;

            return null;
        }
    }
}

