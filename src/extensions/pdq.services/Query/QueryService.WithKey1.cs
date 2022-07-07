using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using pdq.common;
using Dapper.Contrib.Extensions;
using System.Linq;

namespace pdq.services
{
    public class Query<TEntity, TKey> :
        Query<TEntity>,
        IQuery<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
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
            var results = new List<TEntity>();
            var conn = this.GetTransient().Connection.GetUnderlyingConnection();
            foreach(var k in keys)
            {
                var r = conn.Get<TEntity>(k);
                if (r == default(TEntity)) continue;
                results.Add(r);
            }
            return results;
        }
    }
}

