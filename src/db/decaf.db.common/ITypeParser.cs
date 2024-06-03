using System;

namespace decaf.db.common;

public interface ITypeParser
{
    /// <summary>
    /// Parse the provided type to the appropriate database type.
    /// </summary>
    /// <typeparam name="T">The type to convert.</typeparam>
    /// <returns>The database type as a string.</returns>
    string Parse<T>();
    
    /// <summary>
    /// Parse the provided type to the appropriate database type.
    /// </summary>
    /// <param name="type">The type to convert.</param>
    /// <returns>The database type as a string.</returns>
    string Parse(Type type);
}