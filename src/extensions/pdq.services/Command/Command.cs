using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using pdq.common;
using pdq.common.Connections;
using pdq.state;
using pdq.common.Utilities.Reflection;

namespace pdq.services
{
    internal class Command<TEntity> :
        Service,
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

        protected IEnumerable<TEntity> Add(IEnumerable<TEntity> items, IEnumerable<string> outputs)
        {
            var first = items.First();
            return ExecuteQuery(q =>
            {
                var query = q.Insert();
                var table = GetTableInfo<TEntity>(q);
                var exec = query.Into(table)
                    .Columns((t) => first)
                    .Values(items);
                foreach (var o in outputs)
                    exec.Output(o);
                NotifyPreExecution(this, q);

                var results = exec.ToList<TEntity>();

                var inputItems = items.ToArray();
                var i = 0;
                foreach (var item in results)
                {
                    var r = inputItems[i];
                    foreach (var o in outputs) r.SetPropertyValueFrom(o, item);
                    i += 1;
                }
                return inputItems;
            });
        }

        /// <inheritdoc/>
        public virtual IEnumerable<TEntity> Add(params TEntity[] toAdd)
            => Add(toAdd?.ToList());

        /// <inheritdoc/>
        public virtual IEnumerable<TEntity> Add(IEnumerable<TEntity> toAdd)
        {
            if (toAdd == null ||
               !toAdd.Any())
                return new List<TEntity>();

            return ExecuteQuery(q =>
            {
                var first = toAdd.First();
                var query = q.Insert()
                    .Into<TEntity>(t => t)
                    .Columns(t => first)
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
                var alias = q.Context.Helpers().GetTableAlias(expression);
                var whereClause = q.Context.Helpers().ParseWhere(expression);

                IUpdateSet<TEntity> executeQ = updateQ.Table<TEntity>(alias)
                    .Set(toUpdate);

                executeQ.Where(whereClause);
                NotifyPreExecution(this, q);
                executeQ.Execute();
            });
        }

        protected void DeleteByKeys<TKey>(IEnumerable<TKey> keys, Action<IEnumerable<TKey>, IQueryContainer, IWhereBuilder> action)
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
                    var query = q.Delete();
                    var table = base.GetTableInfo<TEntity>(q);
                    var exec = query.From(table, "t")
                        .Where(b => action(keyBatch, q, b));
                    NotifyPreExecution(this, q);

                    exec.Execute();
                }

                skip += take;
            } while (skip < numKeys);

            if (this.disposeOnExit) t.Dispose();
        }
    }
}

