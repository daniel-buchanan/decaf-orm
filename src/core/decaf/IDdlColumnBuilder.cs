using System;
using System.Linq.Expressions;

namespace decaf;

public interface IDdlColumnBuilder
{
    IDdlColumnBuilder Named(string name);
    
    IDdlColumnBuilder Named<T>();

    IDdlColumnBuilder Named<T, TValue>(Expression<Func<T, TValue>> expression);

    IDdlColumnBuilder IsNullable();

    IDdlColumnBuilder AsType<T>();

    IDdlColumnBuilder AsType(Type type);
}