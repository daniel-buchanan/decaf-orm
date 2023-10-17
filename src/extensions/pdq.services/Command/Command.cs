using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using pdq.common;
using pdq.common.Connections;
using pdq.state;
using pdq.common.Utilities.Reflection;
using System.Threading.Tasks;
using System.Threading;
using pdq.common.Utilities;

namespace pdq.services
{
    internal class Command<TEntity> :
        Service,
        ICommand<TEntity>
        where TEntity : class, IEntity, new()
    {
        public Command(IPdq pdq) : base(pdq) { }

        protected Command(ITransient transient) : base(transient) { }

        public event EventHandler<PreExecutionEventArgs> OnBeforeExecution
        {
            add => base.preExecution += value;
            remove => base.preExecution -= value;
        }

        public static ICommand<TEntity> Create(ITransient transient) => new Command<TEntity>(transient);

        /// <inheritdoc/>
        public TEntity Add(TEntity toAdd) => AddAsync(toAdd).WaitFor();

        protected async Task<IEnumerable<TEntity>> AddAsync(
            IEnumerable<TEntity> items,
            IEnumerable<string> outputs,
            CancellationToken cancellationToken = default)
        {
            if (items == null ||
               !items.Any())
                return new List<TEntity>();

            var first = items.First();
            return await ExecuteQueryAsync(async (q, c) =>
            {
                var query = q.Insert();
                var table = GetTableInfo<TEntity>(q);
                var exec = query.Into(table)
                    .Columns((t) => first)
                    .Values(items);
                foreach (var o in outputs)
                    exec.Output(o);
                NotifyPreExecution(this, q);

                var results = await exec.ToListAsync<TEntity>(c);

                var inputItems = items.ToArray();
                var i = 0;
                foreach (var item in results)
                {
                    var r = inputItems[i];
                    foreach (var o in outputs) r.SetPropertyValueFrom(o, item);
                    i += 1;
                }
                return inputItems;
            }, cancellationToken);
        }

        /// <inheritdoc/>
        public virtual IEnumerable<TEntity> Add(params TEntity[] toAdd)
            => Add(toAdd?.ToList());

        /// <inheritdoc/>
        public virtual IEnumerable<TEntity> Add(IEnumerable<TEntity> toAdd)
            => AddAsync(toAdd).WaitFor();

        /// <inheritdoc/>
        public void Delete(Expression<Func<TEntity, bool>> expression)
            => DeleteAsync(expression).WaitFor();

        /// <inheritdoc/>
        public void Update(TEntity toUpdate, Expression<Func<TEntity, bool>> expression)
            => UpdateAsync(toUpdate, expression).WaitFor();

        /// <inheritdoc/>
        public void Update(dynamic toUpdate, Expression<Func<TEntity, bool>> expression)
            => UpdateAsync(toUpdate, expression).WaitFor();

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
            var numKeys = keys?.Count() ?? 0;
            if (numKeys == 0) return;

            var t = this.GetTransient();
            const int take = 100;
            var skip = 0;

            do
            {
                var keyBatch = keys.Skip(skip).Take(take);

                using (var q = await t.QueryAsync(cancellationToken))
                {
                    var query = q.Delete();
                    var table = base.GetTableInfo<TEntity>(q);
                    var exec = query.From(table, "t")
                        .Where(b => action(keyBatch, q, b));
                    NotifyPreExecution(this, q);

                    await exec.ExecuteAsync(cancellationToken);
                }

                skip += take;
            } while (skip < numKeys);

            if (this.disposeOnExit) t.Dispose();
        }

        public async Task<TEntity> AddAsync(
            TEntity toAdd,
            CancellationToken cancellationToken = default)
        {
            var result = await AddAsync(new[] { toAdd }, new string[0], cancellationToken);
            return result.First();
        }

        public async Task<IEnumerable<TEntity>> AddAsync(
            IEnumerable<TEntity> toAdd,
            CancellationToken cancellationToken = default)
        {
            if (toAdd == null ||
               !toAdd.Any())
                return new List<TEntity>();

            return await ExecuteQueryAsync(async (q, c) =>
            {
                var first = toAdd.First();
                var query = q.Insert()
                    .Into<TEntity>(t => t)
                    .Columns(t => first)
                    .Values(toAdd);
                NotifyPreExecution(this, q);
                await query.ExecuteAsync(c);
                return toAdd;
            });
        }

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
            await ExecuteQueryAsync(async (q, c) =>
            {
                var query = q.Delete()
                    .From<TEntity>()
                    .Where(expression);
                NotifyPreExecution(this, q);
                await query.ExecuteAsync();
            }, cancellationToken);
        }
    }
}

