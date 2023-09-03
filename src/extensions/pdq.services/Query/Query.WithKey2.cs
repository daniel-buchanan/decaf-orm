using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using pdq.common;
using System.Linq;
using pdq.common.Connections;

namespace pdq.services
{
    internal class Query<TEntity, TKey1, TKey2> :
        Query<TEntity>,
        IQuery<TEntity, TKey1, TKey2>
        where TEntity : class, IEntity<TKey1, TKey2>, new()
    {
        public Query(IPdq pdq) : base(pdq) { }

        private Query(ITransient transient) : base(transient) { }

        public new static IQuery<TEntity, TKey1, TKey2> Create(ITransient transient)
            => new Query<TEntity, TKey1, TKey2>(transient);

        /// <inheritdoc/>
        public new IEnumerable<TEntity> All() => base.All();

        /// <inheritdoc/>
        public new IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> query)
            => base.Get(query);

        /// <inheritdoc/>
        public TEntity Get(TKey1 key1, TKey2 key2)
            => Get(new[] { CompositeKeyValue.Create(key1, key2) }).FirstOrDefault();

        /// <inheritdoc/>
        public IEnumerable<TEntity> Get(params ICompositeKeyValue<TKey1, TKey2>[] keys)
            => Get(keys?.AsEnumerable());

        /// <inheritdoc/>
        public IEnumerable<TEntity> Get(IEnumerable<ICompositeKeyValue<TKey1, TKey2>> keys)
        {
            var tmp = new TEntity();
            
            return GetByKeys(keys, (keyBatch, q, b) =>
            {
                var key1Name = GetKeyColumnName<TEntity>(q, tmp.KeyMetadata.ComponentOne);
                var key2Name = GetKeyColumnName<TEntity>(q, tmp.KeyMetadata.ComponentTwo);
                b.ClauseHandling.DefaultToOr();
                foreach (var k in keyBatch)
                {
                    b.And(ab =>
                    {
                        ab.Column(key1Name, "t").Is().EqualTo(k.ComponentOne);
                        ab.Column(key2Name, "t").Is().EqualTo(k.ComponentTwo);
                    });
                }
            });
        }
    }
}

