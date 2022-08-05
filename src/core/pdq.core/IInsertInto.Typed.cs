using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace pdq
{
    public interface IInsertInto<T>
    {
        IInsertValues<T> Columns(Expression<Func<T, dynamic>> columns);
    }
}

