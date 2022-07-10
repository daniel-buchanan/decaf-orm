using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using pdq.common;

namespace pdq.services
{
    public class Service<TEntity, TKey1, TKey2, TKey3> :
        IService<TEntity, TKey1, TKey2, TKey3>
        where TEntity : class, IEntity<TKey1, TKey2, TKey3>
    {
        private readonly IQuery<TEntity, TKey1, TKey2, TKey3> query;
        private readonly ICommand<TEntity, TKey1, TKey2, TKey3> command;

        public Service(
            IQuery<TEntity, TKey1, TKey2, TKey3> query,
            ICommand<TEntity, TKey1, TKey2, TKey3> command)
        {
            this.query = query;
            this.command = command;
        }

        private Service(ITransient transient)
        {
            this.query = Query<TEntity, TKey1, TKey2, TKey3>.Create(transient);
            this.command = Command<TEntity, TKey1, TKey2, TKey3>.Create(transient);
        }

        public static IService<TEntity, TKey1, TKey2, TKey3> Create(ITransient transient)
            => new Service<TEntity, TKey1, TKey2, TKey3>(transient);

        /// <inheritdoc/>
        public TEntity Add(TEntity toAdd)
            => this.command.Add(toAdd);

        /// <inheritdoc/>
        public IEnumerable<TEntity> All()
            => this.query.All();

        /// <inheritdoc/>
        public void Delete(TKey1 key1, TKey2 key2, TKey3 key3)
            => this.command.Delete(key1, key2, key3);

        /// <inheritdoc/>
        public void Delete(params ICompositeKeyValue<TKey1, TKey2, TKey3>[] keys)
            => this.command.Delete(keys);

        /// <inheritdoc/>
        public void Delete(IEnumerable<ICompositeKeyValue<TKey1, TKey2, TKey3>> keys)
            => this.command.Delete(keys);

        /// <inheritdoc/>
        public void Delete(Expression<Func<TEntity, bool>> expression)
            => this.command.Delete(expression);

        /// <inheritdoc/>
        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> query)
            => this.query.Get(query);

        /// <inheritdoc/>
        public TEntity Get(TKey1 key1, TKey2 key2, TKey3 key3)
            => this.query.Get(key1, key2, key3);

        /// <inheritdoc/>
        public IEnumerable<TEntity> Get(params ICompositeKeyValue<TKey1, TKey2, TKey3>[] keys)
            => this.query.Get(keys);

        /// <inheritdoc/>
        public IEnumerable<TEntity> Get(IEnumerable<ICompositeKeyValue<TKey1, TKey2, TKey3>> keys)
            => this.query.Get(keys);

        /// <inheritdoc/>
        public void Update(dynamic toUpdate, TKey1 key1, TKey2 key2, TKey3 key3)
            => this.command.Update(toUpdate, key1, key2, key3);

        /// <inheritdoc/>
        public void Update(TEntity toUpdate, Expression<Func<TEntity, bool>> expression)
            => this.command.Update(toUpdate, expression);

        /// <inheritdoc/>
        public void Update(dynamic toUpdate, Expression<Func<TEntity, bool>> expression)
            => this.command.Update(toUpdate, expression);
    }
}

