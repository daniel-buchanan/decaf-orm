using System;
using System.Linq.Expressions;

namespace decaf;

public interface IInsertInto
{
    IInsertValues Columns(Expression<Func<IInsertColumnBuilder, dynamic>> columns);
}