using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using pdq.common;
using System.Linq;

namespace pdq.services
{
    internal class Query<TEntity, TKey1, TKey2, TKey3> :
        Query<TEntity>,
        IQuery<TEntity, TKey1, TKey2, TKey3>
        where TEntity : class, IEntity<TKey1, TKey2, TKey3>, new()
    {
        public Query(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        private Query(ITransient transient) : base(transient) { }

        public new static IQuery<TEntity, TKey1, TKey2, TKey3> Create(ITransient transient)
            => new Query<TEntity, TKey1, TKey2, TKey3>(transient);

        /// <inheritdoc/>
        public new IEnumerable<TEntity> All() => base.All();

        /// <inheritdoc/>
        public new IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> query)
            => base.Get(query);

        /// <inheritdoc/>
        public TEntity Get(TKey1 key1, TKey2 key2, TKey3 key3)
            => Get(new[] { CompositeKeyValue.Create(key1, key2, key3) }).FirstOrDefault();

        /// <inheritdoc/>
        public IEnumerable<TEntity> Get(params ICompositeKeyValue<TKey1, TKey2, TKey3>[] keys)
            => Get(keys?.AsEnumerable());

        /// <inheritdoc/>
        public IEnumerable<TEntity> Get(IEnumerable<ICompositeKeyValue<TKey1, TKey2, TKey3>> keys)
        {
            var tmp = new TEntity();
            var entityType = typeof(TEntity);
            var key1Prop = entityType.GetProperty(tmp.KeyMetadata.ComponentOne.Name);
            var key1Name = base.reflectionHelper.GetFieldName(key1Prop);
            var key2Prop = entityType.GetProperty(tmp.KeyMetadata.ComponentTwo.Name);
            var key2Name = base.reflectionHelper.GetFieldName(key2Prop);
            var key3Prop = entityType.GetProperty(tmp.KeyMetadata.ComponentThree.Name);
            var key3Name = base.reflectionHelper.GetFieldName(key3Prop);

            return GetByKeys(keys, (keyBatch, b) =>
            {
                b.ClauseHandling.DefaultToOr();
                foreach (var k in keyBatch)
                {
                    b.And(ab =>
                    {
                        ab.Column(key1Name, "t").Is().EqualTo(k.ComponentOne);
                        ab.Column(key2Name, "t").Is().EqualTo(k.ComponentTwo);
                        ab.Column(key3Name, "t").Is().EqualTo(k.ComponentThree);
                    });
                }
            });
        }
    }
}

