using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using pdq.common;
using Dapper.Contrib.Extensions;

namespace pdq.services
{
    public class Query<TEntity> : IQuery<TEntity>
        where TEntity : class, IEntity
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
    }
}

