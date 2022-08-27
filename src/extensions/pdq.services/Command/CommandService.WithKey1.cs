using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using pdq.common;
using pdq.state;
using pdq.state.Conditionals;
using pdq.state.Utilities;
using static pdq.Attributes.IgnoreColumnFor;

namespace pdq.services
{
    internal class Command<TEntity, TKey> :
        Command<TEntity>,
        ICommand<TEntity, TKey>
        where TEntity : class, IEntity<TKey>, new()
    {
        const string Alias = "t";

        public Command(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        private Command(ITransient transient) : base(transient) { }

        public static new ICommand<TEntity, TKey> Create(ITransient transient) => new Command<TEntity, TKey>(transient);

        /// <inheritdoc/>
        public new TEntity Add(TEntity toAdd)
        {
            return ExecuteQuery<TEntity>(q =>
            {
                var internalContext = q as IQueryContextInternal;
                GetTableAndKeyDetails(q, out var table, out _, out _);

                var query = q.Insert()
                    .Into(table)
                    .Columns((t) => toAdd)
                    .Value(toAdd)
                    .Output(toAdd.KeyMetadata.Name);
                NotifyPreExecution(this, q);

                var result = query.FirstOrDefault<TEntity>();

                var newValue = internalContext.ReflectionHelper.GetPropertyValue(result, toAdd.KeyMetadata.Name);
                toAdd.SetProperty(toAdd.KeyMetadata.Name, newValue);

                return toAdd;
            });
        }

        /// <inheritdoc/>
        public void Delete(TKey key) => Delete(new[] { key });

        /// <inheritdoc/>
        public void Delete(params TKey[] keys) => Delete(keys?.AsEnumerable());

        /// <inheritdoc/>
        public void Delete(IEnumerable<TKey> keys)
        {
            var numKeys = keys?.Count() ?? 0;
            if (numKeys == 0) return;

            var t = this.GetTransient();
            var tmp = new TEntity();
            const int take = 100;
            var skip = 0;

            do
            {
                var keyBatch = keys.Skip(skip).Take(take);

                using (var q = t.Query())
                {
                    GetTableAndKeyDetails(q, out var table, out var column, out _);
                    q.Delete()
                        .From(table)
                        .Where(b =>
                        {
                            b.Column(column).Is().In(keyBatch);
                        })
                        .Execute();
                }

                skip += take;
            } while (skip < numKeys);

            if (this.disposeOnExit) t.Dispose();
        }

        /// <inheritdoc/>
        public void Update(TEntity toUpdate)
        {
            var reflectionHelper = new ReflectionHelper();
            var keyValue = (TKey)reflectionHelper.GetPropertyValue(toUpdate, toUpdate.KeyMetadata.Name);
            Update(toUpdate, keyValue);
        }

        /// <inheritdoc/>
        public void Update(dynamic toUpdate, TKey key)
        {
            ExecuteQuery(q =>
            {
                var internalContext = q as IQueryContextInternal;
                GetTableAndKeyDetails(q, out var table, out var column, out _);

                Action<IWhereBuilder> clause = b =>
                {
                    b.Column(column).Is().EqualTo(key);
                };

                var query = q.Update()
                    .Table(table)
                    .Set(toUpdate)
                    .Where(clause);
                NotifyPreExecution(this, q);
                query.Execute();
            });
        }

        /// <inheritdoc/>
        public new void Update(dynamic toUpdate, Expression<Func<TEntity, bool>> expression)
        {
            ExecuteQuery(q =>
            {
                var internalContext = q as IQueryContextInternal;
                GetTableAndKeyDetails(q, out var table, out var keyColumn, out _);
                Expression<Func<dynamic>> propExpression = () => toUpdate;
                var properties = internalContext.Helpers().GetPropertyInformation(propExpression);

                var partial = q.Update()
                    .Table(table)
                    .Set(toUpdate);

                var clause = internalContext.Parsers.Where.Parse(expression, internalContext);
                (q as IUpdateQueryContext).Where(clause);

                NotifyPreExecution(this, q);
                partial.Execute();
            });
        }

        private void GetTableAndKeyDetails(IQuery q, out string table, out string keyColumn, out object keyValue, TEntity toUpdate = null)
        {
            var tmp = new TEntity();
            var internalQuery = q as IQueryInternal;
            var internalContext = internalQuery.Context as IQueryContextInternal;
            table = internalContext.Helpers().GetTableName<TEntity>();
            var prop = typeof(TEntity).GetProperty(tmp.KeyMetadata.Name);
            keyColumn = internalContext.ReflectionHelper.GetFieldName(prop);
            keyValue = internalContext.ReflectionHelper.GetPropertyValue(toUpdate, keyColumn);
        }
    }
}

