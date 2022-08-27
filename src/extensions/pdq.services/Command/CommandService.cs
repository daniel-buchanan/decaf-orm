using System;
using System.Linq.Expressions;
using pdq.common;
using pdq.state;

namespace pdq.services
{
    internal class Command<TEntity> :
        ServiceBase,
        ICommand<TEntity>
        where TEntity : class, IEntity, new()
    {
        public Command(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        protected Command(ITransient transient) : base(transient) { }

        public event EventHandler<PreExecutionEventArgs> PreExecution;

        public static ICommand<TEntity> Create(ITransient transient) => new Command<TEntity>(transient);

        /// <inheritdoc/>
        public TEntity Add(TEntity toAdd)
        {
            var t = this.GetTransient();
            using(var q = t.Query())
            {
                var query = q.Insert()
                    .Into<TEntity>()
                    .Columns(e => toAdd)
                    .Value(toAdd);
                NotifyPreExecution(this, q);
                query.Execute();
            }

            if (this.disposeOnExit) t.Dispose();

            return toAdd;
        }

        /// <inheritdoc/>
        public void Delete(Expression<Func<TEntity, bool>> expression)
        {
            ExecuteQuery(q =>
            {
                var query = q.Delete()
                    .From<TEntity>()
                    .Where(expression);
                NotifyPreExecution(this, q);
                query.Execute();
            });
        }

        /// <inheritdoc/>
        public void Update(TEntity toUpdate, Expression<Func<TEntity, bool>> expression)
        {
            ExecuteQuery(q =>
            {
                var query = q.Update()
                    .Table<TEntity>()
                    .Set(toUpdate)
                    .Where(expression);
                NotifyPreExecution(this, q);
                query.Execute();
            });
        }

        /// <inheritdoc/>
        public void Update(dynamic toUpdate, Expression<Func<TEntity, bool>> expression)
        {
            ExecuteQuery(q =>
            {
                var internalQuery = q as IQueryInternal;
                var internalContext = internalQuery.Context as IQueryContextInternal;
                var table = internalContext.Helpers().GetTableName<TEntity>();
                var alias = internalContext.Helpers().GetTableAlias(expression);

                var query = q.Update()
                    .Table(table, alias)
                    .Set(toUpdate)
                    .Where(expression);
                NotifyPreExecution(this, q);
                query.Execute();
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

