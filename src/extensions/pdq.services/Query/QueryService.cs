using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using pdq.common;

namespace pdq.services
{
    internal class Query<TEntity> :
        ServiceBase,
        IQuery<TEntity>
        where TEntity : class, IEntity
    {
        public event EventHandler<PreExecutionEventArgs> PreExecution;

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

        protected void NotifyPreExecution(object sender, IQuery query)
        {
            var internalQuery = query as IQueryInternal;
            var args = new PreExecutionEventArgs(internalQuery.Context);
            PreExecution?.Invoke(sender, args);
        }
    }
}

