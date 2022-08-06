using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using pdq.common;

namespace pdq
{
    public interface IInsertValues<T> : IExecute
    {
        IInsertValues<T> Value(Expression<Func<T>> value);
        IInsertValues<T> Values(Expression<Func<IEnumerable<T>>> values);
        IInsertValues<T> From(Action<ISelect> query);
    }
}

