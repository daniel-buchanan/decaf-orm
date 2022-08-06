using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using pdq.common;

namespace pdq
{
    public interface IInsertValues : IExecute
    {
        IInsertValues Value(Expression<Func<dynamic>> value);
        IInsertValues Values(Expression<Func<IEnumerable<dynamic>>> values);
        IInsertValues From(Action<ISelect> query);
    }
}

