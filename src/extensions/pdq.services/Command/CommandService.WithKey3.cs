using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using pdq.common;
using pdq.state;
using pdq.state.Utilities;

namespace pdq.services
{
    internal class Command<TEntity, TKey1, TKey2, TKey3> :
        Command<TEntity>,
        ICommand<TEntity, TKey1, TKey2, TKey3>
        where TEntity : class, IEntity<TKey1, TKey2, TKey3>, new()
    {
        public Command(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        private Command(ITransient transient) : base(transient) { }

        public static new ICommand<TEntity, TKey1, TKey2, TKey3> Create(ITransient transient)
            => new Command<TEntity, TKey1, TKey2, TKey3>(transient);

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
                    .Output(toAdd.KeyMetadata.ComponentOne.Name)
                    .Output(toAdd.KeyMetadata.ComponentTwo.Name)
                    .Output(toAdd.KeyMetadata.ComponentThree.Name);
                NotifyPreExecution(this, q);

                var result = query.FirstOrDefault<TEntity>();

                var newValueOne = internalContext.ReflectionHelper.GetPropertyValue(result, toAdd.KeyMetadata.ComponentOne.Name);
                toAdd.SetProperty(toAdd.KeyMetadata.ComponentOne.Name, newValueOne);

                var newValueTwo = internalContext.ReflectionHelper.GetPropertyValue(result, toAdd.KeyMetadata.ComponentTwo.Name);
                toAdd.SetProperty(toAdd.KeyMetadata.ComponentTwo.Name, newValueTwo);

                var newValueThree = internalContext.ReflectionHelper.GetPropertyValue(result, toAdd.KeyMetadata.ComponentThree.Name);
                toAdd.SetProperty(toAdd.KeyMetadata.ComponentThree.Name, newValueThree);

                return toAdd;
            });
        }

        /// <inheritdoc/>
        public void Delete(TKey1 key1, TKey2 key2, TKey3 key3)
            => Delete(new[] { new CompositeKeyValue<TKey1, TKey2, TKey3>(key1, key2, key3) });

        /// <inheritdoc/>
        public void Delete(params ICompositeKeyValue<TKey1, TKey2, TKey3>[] keys)
            => Delete(keys?.AsEnumerable());

        /// <inheritdoc/>
        public void Delete(IEnumerable<ICompositeKeyValue<TKey1, TKey2, TKey3>> keys)
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
                    GetTableAndKeyDetails(q, out var table, out var key, out _);
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
                                        a.Column(key.ComponentOne.Name).Is().EqualTo(k.ComponentOne);
                                        a.Column(key.ComponentTwo.Name).Is().EqualTo(k.ComponentTwo);
                                        a.Column(key.ComponentThree.Name).Is().EqualTo(k.ComponentThree);
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

        public void Update(dynamic toUpdate, TKey1 key1, TKey2 key2, TKey3 key3)
        {
            var temp = new TEntity();
            var parameterExpression = Expression.Parameter(typeof(TEntity), "t");
            var keyOneConstantExpression = Expression.Constant(key1);
            var keyTwoConstantExpression = Expression.Constant(key2);
            var keyThreeConstantExpression = Expression.Constant(key3);
            var keyOnePropertyExpression = Expression.Property(parameterExpression, temp.KeyMetadata.ComponentOne.Name);
            var keyOneEqualsExpression = Expression.Equal(keyOnePropertyExpression, keyOneConstantExpression);
            var keyTwoPropertyExpression = Expression.Property(parameterExpression, temp.KeyMetadata.ComponentTwo.Name);
            var keyTwoEqualsExpression = Expression.Equal(keyTwoPropertyExpression, keyTwoConstantExpression);
            var keyThreePropertyExpression = Expression.Property(parameterExpression, temp.KeyMetadata.ComponentThree.Name);
            var keyThreeEqualsExpression = Expression.Equal(keyThreePropertyExpression, keyThreeConstantExpression);
            var andExpression = Expression.And(keyOneEqualsExpression, keyTwoEqualsExpression);
            var nestedAndExpression = Expression.And(andExpression, keyThreeEqualsExpression);
            var lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(nestedAndExpression, parameterExpression);

            Update(toUpdate, lambdaExpression);
        }

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

        public void Update(TEntity toUpdate)
        {
            var reflectionHelper = new ReflectionHelper();
            var keyValueOne = (TKey1)reflectionHelper.GetPropertyValue(toUpdate, toUpdate.KeyMetadata.ComponentOne.Name);
            var keyValueTwo = (TKey2)reflectionHelper.GetPropertyValue(toUpdate, toUpdate.KeyMetadata.ComponentTwo.Name);
            var keyValueThree = (TKey3)reflectionHelper.GetPropertyValue(toUpdate, toUpdate.KeyMetadata.ComponentThree.Name);
            Update(toUpdate, keyValueOne, keyValueTwo, keyValueThree);
        }

        private void GetTableAndKeyDetails(
            IQuery q,
            out string table,
            out ICompositeKeyTriple key,
            out ICompositeKeyValue<TKey1, TKey2, TKey3> keyValue,
            TEntity toUpdate = null)
        {
            var tmp = new TEntity();
            var internalQuery = q as IQueryInternal;
            var internalContext = internalQuery.Context as IQueryContextInternal;
            table = internalContext.Helpers().GetTableName<TEntity>();

            var propOne = typeof(TEntity).GetProperty(tmp.KeyMetadata.ComponentOne.Name);
            var keyColumnOne = internalContext.ReflectionHelper.GetFieldName(propOne);
            var keyValueOne = (TKey1)internalContext.ReflectionHelper.GetPropertyValue(toUpdate, keyColumnOne);

            var propTwo = typeof(TEntity).GetProperty(tmp.KeyMetadata.ComponentTwo.Name);
            var keyColumnTwo = internalContext.ReflectionHelper.GetFieldName(propTwo);
            var keyValueTwo = (TKey2)internalContext.ReflectionHelper.GetPropertyValue(toUpdate, keyColumnTwo);

            var propThree = typeof(TEntity).GetProperty(tmp.KeyMetadata.ComponentThree.Name);
            var keyColumnThree = internalContext.ReflectionHelper.GetFieldName(propThree);
            var keyValueThree = (TKey3)internalContext.ReflectionHelper.GetPropertyValue(toUpdate, keyColumnThree);

            key = new CompositeKeyTriple
            {
                ComponentOne = new KeyMetadata<TKey1>() { Name = keyColumnOne },
                ComponentTwo = new KeyMetadata<TKey2>() { Name = keyColumnTwo },
                ComponentThree = new KeyMetadata<TKey3>() { Name = keyColumnThree }
            };

            keyValue = new CompositeKeyValue<TKey1, TKey2, TKey3>(keyValueOne, keyValueTwo, keyValueThree);
        }
    }
}

