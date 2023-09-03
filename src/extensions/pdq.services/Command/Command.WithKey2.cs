using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using pdq.common;
using pdq.common.Connections;

namespace pdq.services
{
    internal class Command<TEntity, TKey1, TKey2> :
        Command<TEntity>,
        ICommand<TEntity, TKey1, TKey2>
        where TEntity : class, IEntity<TKey1, TKey2>, new()
    {
        public Command(IPdq pdq) : base(pdq) { }

        private Command(ITransient transient) : base(transient) { }

        public static new ICommand<TEntity, TKey1, TKey2> Create(ITransient transient)
            => new Command<TEntity, TKey1, TKey2>(transient);

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
            return Add(toAdd, new[] { first.KeyMetadata.ComponentOne.Name, first.KeyMetadata.ComponentTwo.Name });
        }

        /// <inheritdoc/>
        public void Delete(TKey1 key1, TKey2 key2) => Delete(new[] { new CompositeKeyValue<TKey1, TKey2>(key1, key2) });

        /// <inheritdoc/>
        public void Delete(params ICompositeKeyValue<TKey1, TKey2>[] keys) => Delete(keys?.AsEnumerable());

        /// <inheritdoc/>
        public void Delete(IEnumerable<ICompositeKeyValue<TKey1, TKey2>> keys)
        {
            DeleteByKeys(keys, (keyBatch, q, b) =>
            {
                b.ClauseHandling.DefaultToOr();
                GetKeyColumnNames<TEntity, TKey1, TKey2>(q, out var keyColumnOne, out var keyColumnTwo);
                foreach(var k in keyBatch)
                {
                    b.And(a =>
                    {
                        a.Column(keyColumnOne).Is().EqualTo(k.ComponentOne);
                        a.Column(keyColumnTwo).Is().EqualTo(k.ComponentTwo);
                    });
                }
            });
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
            var andExpression = Expression.AndAlso(keyOneEqualsExpression, keyTwoEqualsExpression);
            var lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(andExpression, parameterExpression);

            Update(toUpdate, lambdaExpression);
        }

        /// <inheritdoc/>
        public void Update(TEntity toUpdate)
        {
            var key = toUpdate.GetKeyValue();
            Update(toUpdate, key.ComponentOne, key.ComponentTwo);
        }
    }
}

