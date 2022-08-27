using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using pdq.common;
using System.Linq;

namespace pdq.services
{
    internal class Query<TEntity, TKey1, TKey2> :
        Query<TEntity>,
        IQuery<TEntity, TKey1, TKey2>
        where TEntity : class, IEntity<TKey1, TKey2>, new()
    {
        public Query(IUnitOfWork unitOfWork) : base(unitOfWork) { }

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
            var numKeys = keys?.Count() ?? 0;
            if (numKeys == 0) return Enumerable.Empty<TEntity>();

            var t = this.GetTransient();
            const int take = 100;
            var skip = 0;
            var results = new List<TEntity>();

            var table = base.reflectionHelper.GetTableName<TEntity>();
            var tmp = new TEntity();
            var entityType = typeof(TEntity);
            var key1Prop = entityType.GetProperty(tmp.KeyMetadata.ComponentOne.Name);
            var key1Name = base.reflectionHelper.GetFieldName(key1Prop);
            var key2Prop = entityType.GetProperty(tmp.KeyMetadata.ComponentTwo.Name);
            var key2Name = base.reflectionHelper.GetFieldName(key2Prop);

            do
            {
                var keyBatch = keys.Skip(skip).Take(take);

                using (var q = t.Query())
                {
                    var sel = q.Select()
                        .From(table, "t")
                        .Where(b =>
                        {
                            b.ClauseHandling.DefaultToOr();
                            foreach(var k in keyBatch)
                            {
                                b.And(ab =>
                                {
                                    ab.Column(key1Name, "t").Is().EqualTo(k.ComponentOne);
                                    ab.Column(key2Name, "t").Is().EqualTo(k.ComponentTwo);
                                });
                            }
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

