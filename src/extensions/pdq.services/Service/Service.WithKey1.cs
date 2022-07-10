using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using pdq.common;

namespace pdq.services
{
    public class Service<TEntity, TKey> :
        IService<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        private readonly IQuery<TEntity, TKey> query;
        private readonly ICommand<TEntity, TKey> command;

        public Service(
            IQuery<TEntity, TKey> query,
            ICommand<TEntity, TKey> command)
        {
            this.query = query;
            this.command = command;
        }

        private Service(ITransient transient)
        {
            this.query = Query<TEntity, TKey>.Create(transient);
            this.command = Command<TEntity, TKey>.Create(transient);
        }

        public static IService<TEntity, TKey> Create(ITransient transient)
            => new Service<TEntity, TKey>(transient);

        /// <inheritdoc/>
        public TEntity Add(TEntity toAdd)
            => this.command.Add(toAdd);

        /// <inheritdoc/>
        public IEnumerable<TEntity> All()
            => this.query.All();

        /// <inheritdoc/>
        public void Delete(TKey key)
            => this.command.Delete(key);

        /// <inheritdoc/>
        public void Delete(params TKey[] keys)
            => this.command.Delete(keys);

        /// <inheritdoc/>
        public void Delete(IEnumerable<TKey> keys)
            => this.command.Delete(keys);

        /// <inheritdoc/>
        public void Delete(Expression<Func<TEntity, bool>> expression)
            => this.command.Delete(expression);

        /// <inheritdoc/>
        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> query)
            => this.query.Get(query);

        /// <inheritdoc/>
        public TEntity Get(TKey key)
            => this.query.Get(key);

        /// <inheritdoc/>
        public IEnumerable<TEntity> Get(params TKey[] keys)
            => this.query.Get(keys);

        /// <inheritdoc/>
        public IEnumerable<TEntity> Get(IEnumerable<TKey> keys)
            => this.query.Get(keys);

        /// <inheritdoc/>
        public void Update(dynamic toUpdate, TKey key)
            => this.command.Update(toUpdate, key);

        /// <inheritdoc/>
        public void Update(TEntity toUpdate, Expression<Func<TEntity, bool>> expression)
            => this.command.Update(toUpdate, expression);

        /// <inheritdoc/>
        public void Update(dynamic toUpdate, Expression<Func<TEntity, bool>> expression)
            => this.command.Update(toUpdate, expression);
    }
}

