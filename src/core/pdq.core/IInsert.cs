using System;
using System.Linq.Expressions;

namespace pdq
{
    public interface IInsert
    {
        IInsertInto Into(string table, string alias = null);

        IInsertInto<T> Into<T>();

        IInsertInto<T> Into<T>(Expression<Func<T, object>> expression);
    }
}

