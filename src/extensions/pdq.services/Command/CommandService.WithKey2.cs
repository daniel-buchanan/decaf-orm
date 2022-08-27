using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using pdq.common;
using pdq.state;
using pdq.state.Utilities;

namespace pdq.services
{
    internal class Command<TEntity, TKey1, TKey2> :
        Command<TEntity>,
        ICommand<TEntity, TKey1, TKey2>
        where TEntity : class, IEntity<TKey1, TKey2>, new()
    {
        public Command(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        private Command(ITransient transient) : base(transient) { }

        public static new ICommand<TEntity, TKey1, TKey2> Create(ITransient transient)
            => new Command<TEntity, TKey1, TKey2>(transient);

        /// <inheritdoc/>
        public new TEntity Add(TEntity toAdd)
        {
            return ExecuteQuery<TEntity>(q =>
            {
                var internalContext = q as IQueryContextInternal;
                GetTableAndKeyDetails(q, out var table, out _, out _, out _, out _);

                var query = q.Insert()
                    .Into(table)
                    .Columns((t) => toAdd)
                    .Value(toAdd)
                    .Output(toAdd.KeyMetadata.ComponentOne.Name)
                    .Output(toAdd.KeyMetadata.ComponentTwo.Name);
                NotifyPreExecution(this, q);

                var result = query.FirstOrDefault<TEntity>();

                var newValueOne = internalContext.ReflectionHelper.GetPropertyValue(result, toAdd.KeyMetadata.ComponentOne.Name);
                toAdd.SetProperty(toAdd.KeyMetadata.ComponentOne.Name, newValueOne);

                var newValueTwo = internalContext.ReflectionHelper.GetPropertyValue(result, toAdd.KeyMetadata.ComponentTwo.Name);
                toAdd.SetProperty(toAdd.KeyMetadata.ComponentOne.Name, newValueTwo);

                return toAdd;
            });
        }

        /// <inheritdoc/>
        public void Delete(TKey1 key1, TKey2 key2) => Delete(new[] { new CompositeKeyValue<TKey1, TKey2>(key1, key2) });

        /// <inheritdoc/>
        public void Delete(params ICompositeKeyValue<TKey1, TKey2>[] keys) => Delete(keys?.AsEnumerable());

        /// <inheritdoc/>
        public void Delete(IEnumerable<ICompositeKeyValue<TKey1, TKey2>> keys)
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
                    GetTableAndKeyDetails(q, out var table, out var keyColumnOne, out var keyColumnTwo, out _, out _);
                    var query = q.Delete()
                        .From(table)
                        .Where(b =>
                        {
                            b.Or(o =>
                            {
                                foreach (var k in keyBatch)
                                {
                                    o.And(a =>
                                    {
                                        a.Column(keyColumnOne).Is().EqualTo(k.ComponentOne);
                                        a.Column(keyColumnTwo).Is().EqualTo(k.ComponentTwo);
                                    });
                                }
                            });
                        });
                    NotifyPreExecution(this, q);
                    query.Execute();
                }

                skip += take;
            } while (skip < numKeys);

            if (this.disposeOnExit) t.Dispose();
        }

        /// <inheritdoc/>
        public void Update(dynamic toUpdate, TKey1 key1, TKey2 key2)
        {
            var temp = new TEntity();
            var parameterExpression = Expression.Parameter(typeof(TEntity), "t");
            var keyOneConstantExpression = Expression.Constant(key1);
            var keyTwoConstantExpression = Expression.Constant(key2);
            var keyOnePropertyExpression = Expression.Property(parameterExpression, temp.KeyMetadata.ComponentOne.Name);
            var keyOneEqualsExpression = Expression.Equal(keyOnePropertyExpression, keyOneConstantExpression);
            var keyTwoPropertyExpression = Expression.Property(parameterExpression, temp.KeyMetadata.ComponentTwo.Name);
            var keyTwoEqualsExpression = Expression.Equal(keyTwoPropertyExpression, keyTwoConstantExpression);
            var andExpression = Expression.And(keyOneEqualsExpression, keyTwoEqualsExpression);
            var lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(andExpression, parameterExpression);

            Update(toUpdate, lambdaExpression);
        }

        /// <inheritdoc/>
        public new void Update(dynamic toUpdate, Expression<Func<TEntity, bool>> expression)
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
        public void Update(TEntity toUpdate)
        {
            var reflectionHelper = new ReflectionHelper();
            var keyValueOne = (TKey1)reflectionHelper.GetPropertyValue(toUpdate, toUpdate.KeyMetadata.ComponentOne.Name);
            var keyValueTwo = (TKey2)reflectionHelper.GetPropertyValue(toUpdate, toUpdate.KeyMetadata.ComponentTwo.Name);
            Update(toUpdate, keyValueOne, keyValueTwo);
        }

        private void GetTableAndKeyDetails(
            IQuery q,
            out string table,
            out string keyColumnOne,
            out string keyColumnTwo,
            out object keyValueOne,
            out object keyValueTwo,
            TEntity toUpdate = null)
        {
            var tmp = new TEntity();
            var internalQuery = q as IQueryInternal;
            var internalContext = internalQuery.Context as IQueryContextInternal;
            table = internalContext.Helpers().GetTableName<TEntity>();

            var propOne = typeof(TEntity).GetProperty(tmp.KeyMetadata.ComponentOne.Name);
            keyColumnOne = internalContext.ReflectionHelper.GetFieldName(propOne);
            keyValueOne = internalContext.ReflectionHelper.GetPropertyValue(toUpdate, keyColumnOne);

            var propTwo = typeof(TEntity).GetProperty(tmp.KeyMetadata.ComponentTwo.Name);
            keyColumnTwo = internalContext.ReflectionHelper.GetFieldName(propTwo);
            keyValueTwo = internalContext.ReflectionHelper.GetPropertyValue(toUpdate, keyColumnTwo);
        }
    }
}

