using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using decaf.common;
using decaf.Implementation.Ddl;
using decaf.state;
using decaf.state.DDL;

namespace decaf.Implementation;

public class CreateTable : Execute<ICreateTableQueryContext>, ICreateTable
{
    private CreateTable(
        ICreateTableQueryContext context,
        IQueryContainerInternal query) :
        base(query, context)
    {
        Context = context;
        Query.SetContext(Context);
    }

    public static CreateTable Create(
        ICreateTableQueryContext context,
        IQueryContainer query)
        => new CreateTable(context, query as IQueryContainerInternal);
    
    public ICreateTable Named(string name)
    {
        Context.WithName(name);
        return this;
    }

    public ICreateTable WithColumns(params Expression<Action<IDdlColumnBuilder>>[] columns)
    {
        foreach (var c in columns)
        {
            var columnBuilder = new DdlColumnBuilder();
            var func = c.Compile();
            func(columnBuilder);
            Context.AddColumns(columnBuilder.Build());
        }

        return this;
    }

    public ICreateTable WithIndex(string name, string table, params Expression<Action<IDdlColumnBuilder>>[] columns)
    {
        throw new NotImplementedException();
    }

    public ICreateTable WithIndex(string table, params Expression<Action<IDdlColumnBuilder>>[] columns)
    {
        throw new NotImplementedException();
    }

    public ICreateTable WithPrimaryKey(params Expression<Action<IDdlColumnBuilder>>[] columns)
    {
        var cols = new List<IColumnDefinition>();
        foreach (var c in columns)
        {
            var builder = new DdlColumnBuilder();
            var func = c.Compile();
            func(builder);
            cols.Add(builder.Build());
        }

        var pk = PrimaryKeyDefinition.Create(cols.ToArray());
        Context.AddPrimaryKey(pk);

        return this;
    }

    public ICreateTable WithPrimaryKey(string name, params Expression<Action<IDdlColumnBuilder>>[] columns)
    {
        throw new NotImplementedException();
    }
}