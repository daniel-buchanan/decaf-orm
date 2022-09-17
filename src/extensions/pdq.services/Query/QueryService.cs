using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using pdq.common;

namespace pdq.services
{
    internal class Query<TEntity> :
        ServiceBase,
        IQuery<TEntity>
        where TEntity : class, IEntity, new()
    {
        public event EventHandler<PreExecutionEventArgs> PreExecution
        {
            add => base.preExecution += value;
            remove => base.preExecution -= value;
        }

        public Query(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        protected Query(ITransient transient) : base(transient) { }

        public static IQuery<TEntity> Create(ITransient transient) => new Query<TEntity>(transient);

        /// <inheritdoc/>
        public IEnumerable<TEntity> All()
        {
            return ExecuteQuery(q =>
            {
                var sel = q.Select()
                    .From<TEntity>(t => t)
                    .SelectAll<TEntity>(t => t);
                NotifyPreExecution(this, q);
                return sel.AsEnumerable();
            });
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> query)
        {
            return ExecuteQuery(q =>
            {
                var sel = q.Select()
                    .From<TEntity>(t => t)
                    .Where(query)
                    .SelectAll<TEntity>(t => t);
                NotifyPreExecution(this, q);
                return sel.AsEnumerable();
            });
        }

        protected IEnumerable<TEntity> GetByKeys<TKey>(IEnumerable<TKey> keys, Action<IEnumerable<TKey>, IQuery, IWhereBuilder> action)
        {
            var numKeys = keys?.Count() ?? 0;
            if (numKeys == 0) return Enumerable.Empty<TEntity>();

            var t = this.GetTransient();
            const int take = 100;
            var skip = 0;
            var results = new List<TEntity>();

            var table = base.reflectionHelper.GetTableName<TEntity>();

            do
            {
                var keyBatch = keys.Skip(skip).Take(take);

                using (var q = t.Query())
                {
                    var sel = q.Select()
                        .From(table, "t")
                        .Where(b => action(keyBatch, q, b))
                        .SelectAll<TEntity>("t");
                    NotifyPreExecution(this, q);

                    var batchResults = sel.AsEnumerable();
                    results.AddRange(batchResults);
                }

                skip += take;
            } while (skip < numKeys);

            if (this.disposeOnExit) t.Dispose();

            return results;
        }
    }
}

