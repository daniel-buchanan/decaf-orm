using System;
using System.Collections.Generic;
using System.Linq;
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

        public event EventHandler<PreExecutionEventArgs> PreExecution
        {
            add => base.preExecution += value;
            remove => base.preExecution -= value;
        }

        public static ICommand<TEntity> Create(ITransient transient) => new Command<TEntity>(transient);

        /// <inheritdoc/>
        public TEntity Add(TEntity toAdd)
        {
            return ExecuteQuery(q =>
            {
                var query = q.Insert()
                    .Into<TEntity>()
                    .Columns(e => toAdd)
                    .Value(toAdd);
                NotifyPreExecution(this, q);
                query.Execute();

                return toAdd;
            });
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
            => Update(toUpdate as dynamic, expression);

        /// <inheritdoc/>
        public void Update(dynamic toUpdate, Expression<Func<TEntity, bool>> expression)
        {
            ExecuteQuery(q =>
            {
                var internalQuery = q as IQueryInternal;
                var internalContext = internalQuery.Context as IQueryContextInternal;
                var table = base.GetTableInfo<TEntity>(q);
                var alias = internalContext.Helpers().GetTableAlias(expression);

                var query = q.Update()
                    .Table(table, alias)
                    .Set(toUpdate)
                    .Where(expression);
                NotifyPreExecution(this, q);
                query.Execute();
            });
        }

        protected void DeleteByKeys<TKey>(IEnumerable<TKey> keys, Action<IEnumerable<TKey>, IQuery, IWhereBuilder> action)
        {
            var numKeys = keys?.Count() ?? 0;
            if (numKeys == 0) return;

            var t = this.GetTransient();
            const int take = 100;
            var skip = 0;
            var results = new List<TEntity>();

            do
            {
                var keyBatch = keys.Skip(skip).Take(take);

                using (var q = t.Query())
                {
                    var table = base.GetTableInfo<TEntity>(q);
                    var del = q.Delete()
                        .From(table, "t")
                        .Where(b => action(keyBatch, q, b));
                    NotifyPreExecution(this, q);

                    del.Execute();
                }

                skip += take;
            } while (skip < numKeys);

            if (this.disposeOnExit) t.Dispose();
        }
    }
}

