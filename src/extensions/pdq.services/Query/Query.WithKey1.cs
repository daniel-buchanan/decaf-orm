using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using pdq.common;
using System.Linq;
using pdq.common.Connections;

namespace pdq.services
{
    internal class Query<TEntity, TKey> :
        Query<TEntity>,
        IQuery<TEntity, TKey>
        where TEntity : class, IEntity<TKey>, new()
    {
        public Query(IPdq pdq) : base(pdq) { }

        private Query(ITransient transient) : base(transient) { }

        public new static IQuery<TEntity, TKey> Create(ITransient transient) => new Query<TEntity, TKey>(transient);

        /// <inheritdoc/>
        public new IEnumerable<TEntity> All() => base.All();

        /// <inheritdoc/>
        public new IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expression)
            => base.Find(expression);

        /// <inheritdoc/>
        public TEntity Get(TKey key) => Get(new[] { key }).FirstOrDefault();

        /// <inheritdoc/>
        public IEnumerable<TEntity> Get(params TKey[] keys) => Get(keys.ToList());

        /// <inheritdoc/>
        public IEnumerable<TEntity> Get(IEnumerable<TKey> keys)
        {
            var tmp = new TEntity();
         
            return GetByKeys(keys, (keyBatch, q, b) =>
            {
                var keyName = GetKeyColumnName<TEntity>(q, tmp.KeyMetadata);
                b.Column(keyName).Is().In(keyBatch);
            });
        }
    }
}

