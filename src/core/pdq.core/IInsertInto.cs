using System;
using System.Linq.Expressions;

namespace pdq
{
    public interface IInsertInto
    {
        IInsertValues Columns(Expression<Func<IInsertColumnBuilder, dynamic>> columns);
    }
}

