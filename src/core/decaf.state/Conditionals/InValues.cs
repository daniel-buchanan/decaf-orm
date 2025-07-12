using System;
using System.Collections.Generic;
using System.Linq;
using decaf.common;

namespace decaf.state.Conditionals;

public abstract class Values : Where
{
    public abstract Type ValueType { get; protected set; }

    public static InValues<T> In<T>(state.Column column, IEnumerable<T> values)
    {
        return new InValues<T>(column, values);
    }
}

public class InValues<T>(state.Column column, IEnumerable<T>? values) : Values, IInValues
{
    private readonly List<T> values = values?.ToList() ?? [];

    public state.Column Column { get; private set; } = column;

    public IReadOnlyCollection<T> ValueSet => values.AsReadOnly();

    IReadOnlyCollection<object> IInValues.GetValues()
    {
        var result = new List<object>();
        foreach (var o in values)
        {
            result.Add(o!);
        }

        return result.AsReadOnly();
    }


    public sealed override Type ValueType { get; protected set; } = typeof(T);

    public int CountValues => values.Count;
}