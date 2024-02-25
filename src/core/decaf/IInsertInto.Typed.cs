using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace decaf
{
    public interface IInsertInto<T>
    {
        IInsertValues<T> Columns(Expression<Func<T, dynamic>> columns);
    }
}

