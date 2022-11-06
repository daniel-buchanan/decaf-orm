using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using pdq.common;
using pdq.state;
using static pdq.Attributes.IgnoreColumnFor;

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
        public virtual IEnumerable<TEntity> Add(params TEntity[] toAdd)
            => Add(toAdd?.ToList());

        /// <inheritdoc/>
        public virtual IEnumerable<TEntity> Add(IEnumerable<TEntity> toAdd)
        {
            if (toAdd == null ||
               toAdd.Count() == 0)
                return new List<TEntity>();

            return ExecuteQuery(q =>
            {
                var first = toAdd.First();
                var query = q.Insert()
                    .Into<TEntity>()
                    .Columns(e => first)
                    .Values(toAdd);
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
            => UpdateInternal(toUpdate, expression);

        /// <inheritdoc/>
        public void Update(dynamic toUpdate, Expression<Func<TEntity, bool>> expression)
            => UpdateInternal(toUpdate, expression);

        private void UpdateInternal(dynamic toUpdate, Expression<Func<TEntity, bool>> expression)
        {
            ExecuteQuery(q =>
            {
                var updateQ = q.Update();
                var internalQuery = q as IQueryInternal;
                var internalContext = internalQuery.Context as IQueryContextInternal;
                var alias = internalContext.Helpers().GetTableAlias(expression);
                var whereClause = internalContext.Helpers().ParseWhere(expression);

                IUpdateSet<TEntity> executeQ = updateQ.Table<TEntity>(alias)
                    .Set(toUpdate);

                executeQ.Where(whereClause);
                NotifyPreExecution(this, q);
                executeQ.Execute();
            });
        }

        protected void DeleteByKeys<TKey>(IEnumerable<TKey> keys, Action<IEnumerable<TKey>, IQuery, IWhereBuilder> action)
        {
            var numKeys = keys?.Count() ?? 0;
            if (numKeys == 0) return;

            var t = this.GetTransient();
            const int take = 100;
            var skip = 0;

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

