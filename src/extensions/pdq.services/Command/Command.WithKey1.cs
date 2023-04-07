using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using pdq.common;
using pdq.common.Connections;
using pdq.state;

namespace pdq.services
{
    internal class Command<TEntity, TKey> :
        Command<TEntity>,
        ICommand<TEntity, TKey>
        where TEntity : class, IEntity<TKey>, new()
    {
        public Command(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        private Command(ITransient transient) : base(transient) { }

        public static new ICommand<TEntity, TKey> Create(ITransient transient)
            => new Command<TEntity, TKey>(transient);

        /// <inheritdoc/>
        public new TEntity Add(TEntity toAdd)
            => Add(new List<TEntity> { toAdd }).FirstOrDefault();

        /// <inheritdoc/>
        public override IEnumerable<TEntity> Add(params TEntity[] toAdd)
            => Add(toAdd?.ToList());

        /// <inheritdoc/>
        public override IEnumerable<TEntity> Add(IEnumerable<TEntity> toAdd)
        {
            if (toAdd == null ||
               !toAdd.Any())
                return new List<TEntity>();

            var first = toAdd.First();
            return Add(toAdd, new[] { first.KeyMetadata.Name });
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
        public void Update(TEntity item)
        {
            var keyValue = item.GetKeyValue();
            Update(item, keyValue);
        }

        /// <inheritdoc/>
        public void Update(dynamic toUpdate, TKey key)
        {
            var temp = new TEntity();
            var parameterExpression = Expression.Parameter(typeof(TEntity), "t");
            var keyConstantExpression = Expression.Constant(key);
            var keyPropertyExpression = Expression.Property(parameterExpression, temp.KeyMetadata.Name);
            var keyEqualsExpression = Expression.Equal(keyPropertyExpression, keyConstantExpression);
            var lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(keyEqualsExpression, parameterExpression);

            Update(toUpdate, lambdaExpression);
        }

        /// <inheritdoc/>
        public new void Update(dynamic toUpdate, Expression<Func<TEntity, bool>> expression)
        {
            ExecuteQuery(q =>
            {
                var internalQuery = q as IQueryContainerInternal;
                var query = q.Update();
                var internalContext = internalQuery.Context as IQueryContextInternal;
                var table = GetTableInfo<TEntity>(q);
                
                IUpdateSet partial = query.Table(table)
                    .Set(toUpdate);

                var clause = internalContext.Parsers.Where.Parse(expression, internalContext);
                (internalContext as IUpdateQueryContext).Where(clause);

                NotifyPreExecution(this, q);
                partial.Execute();
            });
        }
    }
}

