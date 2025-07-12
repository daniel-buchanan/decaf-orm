using System.Collections.Generic;
using System.Linq;
using decaf.common;
using decaf.common.Utilities;

namespace decaf.state;

internal class UpdateQueryContext : QueryContext, IUpdateQueryContext
{
    private readonly List<IUpdateValueSource> sets;
    private readonly List<Output> outputs;

    private UpdateQueryContext(IAliasManager aliasManager, IHashProvider hashProvider)
        : base(aliasManager, QueryTypes.Update, hashProvider)
    {
        sets = new List<IUpdateValueSource>();
        outputs = new List<Output>();
    }

    /// <summary>
    /// Create a <see cref="IUpdateQueryContext"/> Context.
    /// </summary>
    /// <param name="aliasManager">The <see cref="IAliasManager"/> to use.</param>
    /// <param name="hashProvider">The <see cref="IHashProvider"/> to use.</param>
    /// <returns>A new instance which implements <see cref="IUpdateQueryContext"/>.</returns>
    public static IUpdateQueryContext Create(IAliasManager aliasManager, IHashProvider hashProvider)
        => new UpdateQueryContext(aliasManager, hashProvider);

    /// <inheritdoc/>
    public void Update(ITableTarget target)
    {
        var self = this as IQueryContextExtended;
        var existingTarget = self.QueryTargets.FirstOrDefault(q => q.Alias == target.Alias);
        if (existingTarget != null) return;
        self.AddQueryTarget(target);
    }

    /// <inheritdoc/>
    public void From(IQueryTarget source) => Source = source;

    /// <inheritdoc/>
    public void Where(IWhere whereClause) => WhereClause = whereClause;

    /// <inheritdoc/>
    public void Output(Output outputClause) => outputs.Add(outputClause);

    /// <inheritdoc/>
    public void Set(IUpdateValueSource value) => sets.Add(value);

    /// <inheritdoc/>
    public ITableTarget? Table => QueryTargets.FirstOrDefault() as ITableTarget;

    /// <inheritdoc/>
    public IReadOnlyCollection<IUpdateValueSource> Updates => sets.AsReadOnly();

    /// <inheritdoc/>
    public IQueryTarget? Source { get; private set; }

    /// <inheritdoc/>
    public IWhere? WhereClause { get; private set; }

    /// <inheritdoc/>
    public IReadOnlyCollection<Output> Outputs => outputs.AsReadOnly();
}