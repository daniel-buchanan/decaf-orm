using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using pdq.common;
using System.Linq;

namespace pdq.services
{
    internal class Query<TEntity, TKey> :
        Query<TEntity>,
        IQuery<TEntity, TKey>
        where TEntity : class, IEntity<TKey>, new()
    {
        public Query(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        private Query(ITransient transient) : base(transient) { }

        public new static IQuery<TEntity, TKey> Create(ITransient transient) => new Query<TEntity, TKey>(transient);

        /// <inheritdoc/>
        public new IEnumerable<TEntity> All() => base.All();

        /// <inheritdoc/>
        public new IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> query)
            => base.Get(query);

        /// <inheritdoc/>
        public TEntity Get(TKey key) => Get(new[] { key }).FirstOrDefault();

        /// <inheritdoc/>
        public IEnumerable<TEntity> Get(params TKey[] keys) => Get(keys.ToList());

        /// <inheritdoc/>
        public IEnumerable<TEntity> Get(IEnumerable<TKey> keys)
        {
            var numKeys = keys?.Count() ?? 0;
            if (numKeys == 0) return Enumerable.Empty<TEntity>();

            var t = this.GetTransient();
            const int take = 100;
            var skip = 0;
            var results = new List<TEntity>();

            var table = base.reflectionHelper.GetTableName<TEntity>();
            var tmp = new TEntity();
            var keyProp = typeof(TEntity).GetProperty(tmp.KeyMetadata.Name);
            var keyName = base.reflectionHelper.GetFieldName(keyProp);

            do
            {
                var keyBatch = keys.Skip(skip).Take(take);

                using (var q = t.Query())
                {
                    var sel = q.Select()
                        .From(table, "t")
                        .Where(b =>
                        {
                            b.Column(keyName, "t").Is().In(keyBatch);
                        })
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

