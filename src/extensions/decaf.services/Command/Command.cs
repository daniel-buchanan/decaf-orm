using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using decaf.state;
using decaf.common.Utilities.Reflection;
using System.Threading.Tasks;
using System.Threading;
using decaf.common;
using decaf.common.Connections;
using decaf.common.Utilities;

namespace decaf.services;

internal class Command<TEntity> :
    Service,
    ICommand<TEntity>
    where TEntity : class, IEntity, new()
{
    public Command(IDecaf decaf, ISqlFactory sqlFactory) : base(decaf, sqlFactory) { }

    protected Command(IUnitOfWork unitOfWork) : base(unitOfWork, (unitOfWork as IUnitOfWorkExtended)?.SqlFactory) { }

    public event EventHandler<PreExecutionEventArgs> OnBeforeExecution
    {
        add => PreExecution += value;
        remove => PreExecution -= value;
    }

    public static ICommand<TEntity> Create(IUnitOfWork unitOfWork) 
        => new Command<TEntity>(unitOfWork);

    /// <inheritdoc/>
    public virtual TEntity Add(TEntity toAdd)
        => AddAsync(toAdd).WaitFor();

    /// <inheritdoc/>
    public virtual IEnumerable<TEntity> Add(params TEntity[] toAdd)
        => AddAsync(toAdd?.ToList()).WaitFor();

    /// <inheritdoc/>
    public virtual IEnumerable<TEntity> Add(IEnumerable<TEntity> toAdd)
        => AddAsync(toAdd).WaitFor();

    protected async Task<IEnumerable<TEntity>> AddAsync(
        IEnumerable<TEntity> items,
        IEnumerable<string> outputs,
        CancellationToken cancellationToken = default)
    {
        var inputItems = items?.ToList() ?? new List<TEntity>();
        var outputColumns = outputs?.ToList() ?? new List<string>();

        if (!inputItems.Any())
            return new List<TEntity>();

        var first = inputItems[0];
        return await ExecuteQueryAsync(async (q, c) =>
        {
            var query = q.Insert();
            var table = GetTableInfo<TEntity>(q);
            var exec = query.Into(table)
                .Columns((t) => first)
                .Values(inputItems);
            foreach (var o in outputColumns)
                exec.Output(o);
            NotifyPreExecution(this, q);

            var results = await exec.ToListAsync<TEntity>(c);

            var i = 0;
            foreach (var item in results)
            {
                var r = inputItems[i];
                foreach (var o in outputColumns) r.SetPropertyValueFrom(o, item);
                i += 1;
            }
            return inputItems;
        }, cancellationToken);
    }

    /// <inheritdoc/>
    public void Delete(Expression<Func<TEntity, bool>> expression)
        => DeleteAsync(expression).WaitFor();

    /// <inheritdoc/>
    public void Update(TEntity toUpdate, Expression<Func<TEntity, bool>> expression)
        => UpdateAsync(toUpdate, expression).WaitFor();

    /// <inheritdoc/>
    public void Update(dynamic toUpdate, Expression<Func<TEntity, bool>> expression)
    {
        var t = UpdateAsync(toUpdate, expression);
        t.Wait();
    }

    private async Task UpdateInternalAsync(
        dynamic toUpdate,
        Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        await ExecuteQueryAsync(async (q, c) =>
        {
            var updateQ = q.Update();
            var alias = q.Context.Helpers().GetTableAlias(expression);
            var whereClause = q.Context.Helpers().ParseWhere(expression);

            IUpdateSet<TEntity> executeQ = updateQ.Table<TEntity>(alias)
                .Set(toUpdate);

            executeQ.Where(whereClause);
            NotifyPreExecution(this, q);
            await executeQ.ExecuteAsync(c);
        }, cancellationToken);
    }

    protected async Task DeleteByKeysAsync<TKey>(
        IEnumerable<TKey> keys,
        Action<IEnumerable<TKey>, IQueryContainer, IWhereBuilder> action,
        CancellationToken cancellationToken = default)
    {
        var keysList = keys?.ToList() ?? new List<TKey>();
        var numKeys = keysList.Count;
        if (numKeys == 0) return;

        var t = GetUnitOfWork();
        const int take = 100;
        var skip = 0;

        do
        {
            var keyBatch = keysList.Skip(skip).Take(take);

            using (var q = await t.GetQueryAsync(cancellationToken))
            {
                var query = q.Delete();
                var table = GetTableInfo<TEntity>(q);
                var exec = query.From(table, "t")
                    .Where(b => action(keyBatch, q, b));
                NotifyPreExecution(this, q);

                await exec.ExecuteAsync(cancellationToken);
            }

            skip += take;
        } while (skip < numKeys);

        if (DisposeOnExit) t.Dispose();
    }

    public virtual async Task<TEntity> AddAsync(
        TEntity toAdd,
        CancellationToken cancellationToken = default)
    {
        var result = await AddAsync(new[] { toAdd }, cancellationToken);
        return result.First();
    }

    public virtual async Task<IEnumerable<TEntity>> AddAsync(
        IEnumerable<TEntity> items,
        CancellationToken cancellationToken = default)
        => await AddAsync(items, Array.Empty<string>(), cancellationToken);

    public async Task UpdateAsync(
        TEntity toUpdate,
        Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
        => await UpdateInternalAsync(toUpdate, expression, cancellationToken);

    public async Task UpdateAsync(
        dynamic toUpdate,
        Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
        => await UpdateInternalAsync(toUpdate, expression, cancellationToken);

    public async Task DeleteAsync(
        Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        await ExecuteQueryAsync(async (q, _) =>
        {
            var query = q.Delete()
                .From<TEntity>()
                .Where(expression);
            NotifyPreExecution(this, q);
            await query.ExecuteAsync(cancellationToken);
        }, cancellationToken);
    }
}