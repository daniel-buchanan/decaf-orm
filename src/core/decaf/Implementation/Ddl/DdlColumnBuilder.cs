using System;
using System.Linq.Expressions;
using decaf.state.DDL;

namespace decaf.Implementation.Ddl;

public class DdlColumnBuilder : IDdlColumnBuilder
{
    public string Name { get; private set; }
    public bool Nullable { get; private set; }
    public Type Type { get; private set; }
    
    public IDdlColumnBuilder Named(string name)
    {
        Name = name;
        return this;
    }

    public IDdlColumnBuilder Named<T>()
    {
        Name = typeof(T).Name;
        return this;
    }

    public IDdlColumnBuilder Named<T, TValue>(Expression<Func<T, TValue>> expression)
    {
        throw new NotImplementedException();
    }

    public IDdlColumnBuilder IsNullable()
    {
        Nullable = true;
        return this;
    }

    public IDdlColumnBuilder AsType<T>()
    {
        Type = typeof(T);
        return this;
    }

    public IDdlColumnBuilder AsType(Type type)
    {
        Type = type;
        return this;
    }

    public IColumnDefinition Build()
        => ColumnDefinition.Create(Name, Type, Nullable);
}