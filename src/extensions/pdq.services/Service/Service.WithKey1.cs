using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using pdq.common.Connections;

namespace pdq.services
{
    internal class Service<TEntity, TKey> :
        ExecutionNotifiable,
        IService<TEntity, TKey>
        where TEntity : class, IEntity<TKey>, new()
    {
        private IQuery<TEntity, TKey> Query => GetQuery<IQuery<TEntity, TKey>>();
        private ICommand<TEntity, TKey> Command => GetCommand<ICommand<TEntity, TKey>>();

        public Service(
            IQuery<TEntity, TKey> query,
            ICommand<TEntity, TKey> command)
            : base(query, command) { }

        private Service(ITransient transient)
            : base(transient,
                   Query<TEntity, TKey>.Create,
                   Command<TEntity, TKey>.Create)
        { }

        public static IService<TEntity, TKey> Create(ITransient transient)
            => new Service<TEntity, TKey>(transient);

        /// <inheritdoc/>
        public TEntity Add(TEntity toAdd)
            => this.Command.Add(toAdd);

        /// <inheritdoc/>
        public IEnumerable<TEntity> Add(params TEntity[] toAdd)
            => this.Command.Add(toAdd);

        /// <inheritdoc/>
        public IEnumerable<TEntity> Add(IEnumerable<TEntity> toAdd)
            => this.Command.Add(toAdd);

        /// <inheritdoc/>
        public IEnumerable<TEntity> All()
            => this.Query.All();

        /// <inheritdoc/>
        public Task<IEnumerable<TEntity>> AllAsync(CancellationToken cancellationToken = default)
            => this.Query.AllAsync(cancellationToken);

        /// <inheritdoc/>
        public void Delete(TKey key)
            => this.Command.Delete(key);

        /// <inheritdoc/>
        public void Delete(params TKey[] keys)
            => this.Command.Delete(keys);

        /// <inheritdoc/>
        public void Delete(IEnumerable<TKey> keys)
            => this.Command.Delete(keys);

        /// <inheritdoc/>
        public void Delete(Expression<Func<TEntity, bool>> expression)
            => this.Command.Delete(expression);

        /// <inheritdoc/>
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expression)
            => this.Query.Find(expression);

        /// <inheritdoc/>
        public Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
            => this.Query.FindAsync(expression, cancellationToken);

        /// <inheritdoc/>
        public TEntity Get(TKey key)
            => this.Query.Get(key);

        /// <inheritdoc/>
        public IEnumerable<TEntity> Get(params TKey[] keys)
            => this.Query.Get(keys);

        /// <inheritdoc/>
        public IEnumerable<TEntity> Get(IEnumerable<TKey> keys)
            => this.Query.Get(keys);

        /// <inheritdoc/>
        public void Update(dynamic toUpdate, TKey key)
            => this.Command.Update(toUpdate, key);

        /// <inheritdoc/>
        public void Update(TEntity toUpdate, Expression<Func<TEntity, bool>> expression)
            => this.Command.Update(toUpdate, expression);

        /// <inheritdoc/>
        public void Update(dynamic toUpdate, Expression<Func<TEntity, bool>> expression)
            => this.Command.Update(toUpdate, expression);

        /// <inheritdoc/>
        public void Update(TEntity item)
            => this.Command.Update(item);

        /// <inheritdoc/>
        public async Task UpdateAsync(TEntity item, CancellationToken cancellationToken = default)
            => await this.Command.UpdateAsync(item, cancellationToken);

        /// <inheritdoc/>
        public async Task UpdateAsync(dynamic toUpdate, TKey key, CancellationToken cancellationToken = default)
            => await this.Command.UpdateAsync(toUpdate, key, cancellationToken);

        /// <inheritdoc/>
        public async Task DeleteAsync(TKey key, CancellationToken cancellationToken = default)
            => await this.Command.DeleteAsync(key, cancellationToken);

        /// <inheritdoc/>
        public async Task DeleteAsync(TKey[] keys, CancellationToken cancellationToken = default)
            => await this.Command.DeleteAsync(keys, cancellationToken);

        /// <inheritdoc/>
        public async Task DeleteAsync(IEnumerable<TKey> keys, CancellationToken cancellationToken = default)
            => await this.Command.DeleteAsync(keys, cancellationToken);

        /// <inheritdoc/>
        public async Task<TEntity> AddAsync(TEntity toAdd, CancellationToken cancellationToken = default)
            => await this.Command.AddAsync(toAdd, cancellationToken);

        /// <inheritdoc/>
        public async Task<IEnumerable<TEntity>> AddAsync(IEnumerable<TEntity> toAdd, CancellationToken cancellationToken = default)
            => await this.Command.AddAsync(toAdd, cancellationToken);

        /// <inheritdoc/>
        public async Task UpdateAsync(TEntity toUpdate, Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
            => await this.Command.UpdateAsync(toUpdate, expression, cancellationToken);

        /// <inheritdoc/>
        public async Task UpdateAsync(dynamic toUpdate, Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
            => await this.Command.UpdateAsync(toUpdate, expression, cancellationToken);

        /// <inheritdoc/>
        public async Task DeleteAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
            => await this.Command.DeleteAsync(expression, cancellationToken);
    }
}

