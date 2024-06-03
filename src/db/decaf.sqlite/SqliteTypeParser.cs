using decaf.db.common;

namespace decaf.sqlite;

public class SqliteTypeParser : ITypeParser
{
    public const string DefaultDatatype = "text";
    
    /// <inheritdoc/>
    public string Parse<T>()
        => Parse(typeof(T));

    /// <inheritdoc/>
    public string Parse(Type type)
    {
        if (type == typeof(string))
            return "text";
        if (type == typeof(int))
            return "integer";
        if (type == typeof(decimal) ||
            type == typeof(float) ||
            type == typeof(double) ||
            type == typeof(Single))
            return "float";
        if (type == typeof(bool))
            return "boolean";
        if (type == typeof(byte))
            return "blob";
        if (type == typeof(DateTime) ||
            type == typeof(DateTimeOffset))
            return "timestamp";

        return DefaultDatatype;
    }
}