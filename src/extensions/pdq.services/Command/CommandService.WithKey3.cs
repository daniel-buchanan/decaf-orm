using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using pdq.common;
using pdq.state;

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
            return ExecuteQuery(q =>
            {
                var query = q.Insert();
                var table = GetTableInfo<TEntity>(q);
                var exec = query.Into(table)
                    .Columns((t) => first)
                    .Values(toAdd)
                    .Output(first.KeyMetadata.ComponentOne.Name)
                    .Output(first.KeyMetadata.ComponentTwo.Name)
                    .Output(first.KeyMetadata.ComponentThree.Name);
                NotifyPreExecution(this, q);

                var results = exec.ToList<TEntity>();

                var inputItems = toAdd.ToArray();
                var i = 0;
                foreach (var item in results)
                {
                    var r = inputItems[i];
                    r.SetPropertyFrom(first.KeyMetadata.ComponentOne.Name, item);
                    r.SetPropertyFrom(first.KeyMetadata.ComponentTwo.Name, item);
                    r.SetPropertyFrom(first.KeyMetadata.ComponentThree.Name, item);
                    i += 1;
                }
                return inputItems;
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
            DeleteByKeys(keys, (keyBatch, q, b) =>
            {
                GetKeyColumnNames<TEntity, TKey1, TKey2, TKey3>(q, out var key1Name, out var key2Name, out var key3Name);
                b.ClauseHandling.DefaultToOr();
                foreach(var k in keyBatch)
                {
                    b.And(a =>
                    {
                        a.Column(key1Name).Is().EqualTo(k.ComponentOne);
                        a.Column(key2Name).Is().EqualTo(k.ComponentTwo);
                        a.Column(key3Name).Is().EqualTo(k.ComponentThree);
                    });
                }
            });
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
            var andExpression = Expression.AndAlso(keyOneEqualsExpression, keyTwoEqualsExpression);
            var nestedAndExpression = Expression.AndAlso(andExpression, keyThreeEqualsExpression);
            var lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(nestedAndExpression, parameterExpression);

            Update(toUpdate, lambdaExpression);
        }

        public new void Update(dynamic toUpdate, Expression<Func<TEntity, bool>> expression)
        {
            ExecuteQuery(q =>
            {
                var internalQuery = q as IQueryInternal;
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

        public void Update(TEntity toUpdate)
        {
            var key = toUpdate.GetKeyValue();
            Update(toUpdate, key.ComponentOne, key.ComponentTwo, key.ComponentThree);
        }
    }
}

