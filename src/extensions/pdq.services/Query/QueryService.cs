using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using pdq.common;
using Dapper.Contrib.Extensions;

namespace pdq.services
{
    public class Query<TEntity> :
        ServiceBase,
        IQuery<TEntity>
        where TEntity : class, IEntity
    {
        public Query(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        protected Query(ITransient transient) : base(transient) { }

        public static IQuery<TEntity> Create(ITransient transient) => new Query<TEntity>(transient);

        /// <inheritdoc/>
        public IEnumerable<TEntity> All()
        {
            var conn = this.GetTransient().Connection.GetUnderlyingConnection();
            return conn.GetAll<TEntity>();
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> query)
        {
            using (var q = this.GetTransient().Query())
            {
                return q.Select()
                    .From<TEntity>()
                    .Where(query)
                    .AsEnumerable<TEntity>();
            }
        }
    }
}

