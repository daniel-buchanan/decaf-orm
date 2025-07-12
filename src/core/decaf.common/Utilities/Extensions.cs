using System;

namespace decaf.common.Utilities;

public static class Extensions
{
    public static T? CastAs<T>(this object obj)
    {
        try
        {
            return (T)obj;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error casting object to {typeof(T).Name}: {e.Message}");
            return default;
        }
    }

    public static T ForceCastAs<T>(this object obj)
        where T : class
    {
        var x = obj.CastAs<T>() ?? throw new InvalidCastException($"Cannot cast object of type {obj.GetType().Name} to {typeof(T).Name}");
        return x;
    }

    public static bool TryCastAs<T>(this object obj, out T? result)
    {
        try
        {
            result = (T)obj;
            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }
}