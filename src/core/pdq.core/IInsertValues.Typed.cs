using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using pdq.common;

namespace pdq
{
    public interface IInsertValues<T> : IExecute
    {
        IInsertValues<T> Value(T value);
        IInsertValues<T> Values(IEnumerable<T> values);
        IInsertValues<T> From(Action<ISelect> query);
    }
}

