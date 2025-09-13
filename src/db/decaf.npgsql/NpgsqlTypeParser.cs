using System;
using decaf.db.common;

namespace decaf.npgsql;

public class NpgsqlTypeParser : TypeParser
{
    public const string DefaultDatatype = "text";
    
    /// <inheritdoc/>
    public override string Parse(Type type)
    {
        if (type == typeof(string))
            return "text";
        if (type == typeof(int))
            return "integer";
        if (type == typeof(long))
            return "bigint";
        if (type == typeof(decimal) ||
            type == typeof(float) ||
            type == typeof(double) ||
            type == typeof(Single))
            return "float";
        if (type == typeof(bool))
            return "boolean";
        if (type == typeof(byte[]))
            return "bytea";
        if (type == typeof(DateTime))
            return "timestamp";
        if (type == typeof(DateTimeOffset))
            return "timestampz";
        if (type == typeof(DateOnly))
            return "date";
        if (type == typeof(TimeOnly))
            return "time";
        if (type == typeof(Guid))
            return "uuid";

        return DefaultDatatype;
    }
}