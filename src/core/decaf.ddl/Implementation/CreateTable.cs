using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using decaf.common;
using decaf.common.Utilities.Reflection;
using decaf.ddl.Utilities;
using decaf.Implementation.Execute;
using decaf.state;
using decaf.state.Ddl.Definitions;

namespace decaf.ddl.Implementation;

public class CreateTable : Execute<ICreateTableQueryContext>, ICreateTable
{
    private readonly IExpressionHelper expressionHelper;
    private readonly IReflectionHelper reflectionHelper;

    private CreateTable(
        ICreateTableQueryContext context,
        IQueryContainerInternal query) :
        base(query, context)
    {
        expressionHelper = context.ToInternal().ExpressionHelper;
        reflectionHelper = context.ToInternal().ReflectionHelper;
        query.SetContext(context);
    }

    public static CreateTable Create(
        ICreateTableQueryContext context,
        IQueryContainer query)
        => new CreateTable(context, query as IQueryContainerInternal);

    public static CreateTable<T> Create<T>(
        ICreateTableQueryContext context,
        IQueryContainer query)
        => new CreateTable<T>(context, query as IQueryContainerInternal);

    public ICreateTable FromType<T>(string named = null)
    {
        var tbl = CreateTable.Create<T>(Context, Query);
        if (!string.IsNullOrWhiteSpace(named)) tbl.WithName(named);
        tbl.WithAllColumns();
        tbl.WithPrimaryKey();
        tbl.WithIndexes();
        return this;
    }

    public ICreateTable Named(string name)
    {
        Context.WithName(name);
        return this;
    }

    public ICreateTable WithColumns(params Expression<Action<IDdlColumnBuilder>>[] columns)
    {
        foreach (var c in columns)
        {
            var columnBuilder = new DdlColumnBuilder(expressionHelper);
            var func = c.Compile();
            func(columnBuilder);
            Context.AddColumns(columnBuilder.Build());
        }

        return this;
    }

    public ICreateTable WithColumns<T>()
    {
        var members = reflectionHelper.GetMemberDetails(typeof(T));
        foreach (var m in members)
        {
            var cb = new DdlColumnBuilder(expressionHelper);
            cb.Named(m.NewName ?? m.Name)
                .AsType(m.ValueType);
            Context.AddColumns(cb.Build());
        }

        return this;
    }

    public ICreateTable WithIndex(string name, string table, params Expression<Action<IDdlColumnBuilder>>[] columns)
    {
        var defs = columns.Select(ColumnDefinitionBuilder.Build).ToArray();
        Context.AddIndexes(IndexDefinition.Create(name, table, defs));
        return this;
    }

    public ICreateTable WithIndex(string table, params Expression<Action<IDdlColumnBuilder>>[] columns)
    {
        var defs = columns.Select(ColumnDefinitionBuilder.Build).ToArray();
        Context.AddIndexes(IndexDefinition.Create(null, table, defs));
        return this;
    }

    public ICreateTable WithPrimaryKey(params Expression<Action<IDdlColumnBuilder>>[] columns)
    {
        var cols = new List<IColumnDefinition>();
        foreach (var c in columns)
        {
            var builder = new DdlColumnBuilder(expressionHelper);
            var func = c.Compile();
            func(builder);
            cols.Add(builder.Build());
        }

        var pk = PrimaryKeyDefinition.Create(Context.Name, cols.ToArray());
        Context.AddPrimaryKey(pk);

        return this;
    }

    public ICreateTable WithPrimaryKey(string name, params Expression<Action<IDdlColumnBuilder>>[] columns)
    {
        var defs = columns.Select(ColumnDefinitionBuilder.Build).ToArray();
        Context.AddPrimaryKey(PrimaryKeyDefinition.Create(name, defs.ToArray()));
        return this;
    }
}