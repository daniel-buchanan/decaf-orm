using System;

namespace decaf.db.common;

public abstract class TypeParser : ITypeParser
{
    /// <inheritdoc />
    public string Parse<T>()
        => Parse(typeof(T));

    /// <inheritdoc />
    public abstract string Parse(Type type);
}