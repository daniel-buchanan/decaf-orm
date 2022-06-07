using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using pdq.common;
using Dapper.Contrib.Extensions;
using System.Linq;

namespace pdq.services
{
    public class Query<TEntity, TKey> : IQuery<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        private readonly ITransient transient;

        private Query(ITransient transient)
        {
            this.transient = transient;
        }

        public IEnumerable<TEntity> All()
        {
            var conn = this.transient.Connection.GetUnderlyingConnection();
            return conn.GetAll<TEntity>();
        }

        public void Dispose()
        {
            
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> query)
        {
            var conn = this.transient.Connection.GetUnderlyingConnection();
            
            throw new NotImplementedException();
        }

        public TEntity Get(TKey key)
        {
            var conn = this.transient.Connection.GetUnderlyingConnection();
            return conn.Get<TEntity>(key);
        }

        public IEnumerable<TEntity> Get(params TKey[] keys) => Get(keys.ToList());

        public IEnumerable<TEntity> Get(IEnumerable<TKey> keys)
        {
            var results = new List<TEntity>();
            var conn = this.transient.Connection.GetUnderlyingConnection();
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

