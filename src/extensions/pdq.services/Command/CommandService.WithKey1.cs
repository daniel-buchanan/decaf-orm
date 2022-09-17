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
        public Command(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        private Command(ITransient transient) : base(transient) { }

        public static new ICommand<TEntity, TKey> Create(ITransient transient) => new Command<TEntity, TKey>(transient);

        /// <inheritdoc/>
        public new TEntity Add(TEntity toAdd)
        {
            return ExecuteQuery<TEntity>(q =>
            {
                var internalContext = q as IQueryContextInternal;
                var table = GetTableInfo<TEntity>(q);

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
            DeleteByKeys(keys, (keyBatch, q, b) =>
            {
                GetKeyColumnNames<TEntity, TKey>(q, out var keyName);
                b.Column(keyName).Is().In(keyBatch);
            });
        }

        /// <inheritdoc/>
        public void Update(TEntity toUpdate)
        {
            var keyValue = toUpdate.GetKeyValue();
            Update(toUpdate, keyValue);
        }

        /// <inheritdoc/>
        public void Update(dynamic toUpdate, TKey key)
        {
            var temp = new TEntity();
            var parameterExpression = Expression.Parameter(typeof(TEntity), "t");
            var keyOneConstantExpression = Expression.Constant(key);
            var keyOnePropertyExpression = Expression.Property(parameterExpression, temp.KeyMetadata.Name);
            var keyOneEqualsExpression = Expression.Equal(keyOnePropertyExpression, keyOneConstantExpression);
            var lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(keyOneEqualsExpression, parameterExpression);

            Update(toUpdate, lambdaExpression);
        }

        /// <inheritdoc/>
        public new void Update(dynamic toUpdate, Expression<Func<TEntity, bool>> expression)
        {
            ExecuteQuery(q =>
            {
                var internalContext = q as IQueryContextInternal;
                var table = GetTableInfo<TEntity>(q);
                Expression<Func<dynamic>> propExpression = () => toUpdate;
                
                var partial = q.Update()
                    .Table(table)
                    .Set(toUpdate);

                var clause = internalContext.Parsers.Where.Parse(expression, internalContext);
                (q as IUpdateQueryContext).Where(clause);

                NotifyPreExecution(this, q);
                partial.Execute();
            });
        }
    }
}

