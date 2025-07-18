﻿using System.Collections.Generic;
using System.Linq;
using decaf.common;
using decaf.common.Utilities;

namespace decaf.state;

internal class InsertQueryContext : QueryContext, IInsertQueryContext
{
    private readonly List<Output> outputs;
    private readonly List<Column> columns;

    private InsertQueryContext(
        IAliasManager aliasManager,
        IHashProvider hashProvider)
        : base(aliasManager, QueryTypes.Insert, hashProvider)
    {
        outputs = new List<Output>();
        columns = new List<Column>();
    }

    public static IInsertQueryContext Create(IAliasManager aliasManager, IHashProvider hashProvider)
        => new InsertQueryContext(aliasManager, hashProvider);

    /// <inheritdoc/>
    public ITableTarget? Target
        => QueryTargets.FirstOrDefault()?.CastAs<ITableTarget>();

    /// <inheritdoc/>
    public IReadOnlyCollection<Column> Columns => columns.AsReadOnly();

    /// <inheritdoc/>
    public IInsertValuesSource? Source { get; private set; }

    /// <inheritdoc/>
    public IReadOnlyCollection<Output> Outputs => outputs.AsReadOnly();

    /// <inheritdoc/>
    public void Column(Column column)
    {
        var item = columns.FirstOrDefault(c => c.IsEquivalentTo(column));
        if (item != null) return;

        columns.Add(column);
    }

    /// <inheritdoc/>
    public void From(IInsertValuesSource source) => Source = source;

    /// <inheritdoc/>
    public void Into(ITableTarget target)
    {
        var item = QueryTargets.FirstOrDefault(t => t.IsEquivalentTo(target));
        if (item != null) return;

        var internalContext = this as IQueryContextExtended;
        internalContext.AddQueryTarget(target);
    }

    /// <inheritdoc/>
    public void Output(Output output) => outputs.Add(output);

    /// <inheritdoc/>
    public void Value(object[] value)
    {
        var staticValueSource = Source as IInsertStaticValuesSource;

        if(Source != null && staticValueSource == null) return;

        if (staticValueSource == null)
        {
            staticValueSource = ValueSources.Insert.StaticValuesSource.Create(value.Length);
            Source = staticValueSource;
        }

        staticValueSource.AddValue(value);
    }
}