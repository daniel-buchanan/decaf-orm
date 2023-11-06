using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using pdq.common.Connections;

namespace pdq.services
{
    internal class Service<TEntity, TKey1, TKey2, TKey3> :
        ExecutionNotifiable,
        IService<TEntity, TKey1, TKey2, TKey3>
        where TEntity : class, IEntity<TKey1, TKey2, TKey3>, new()
    {
        private IQuery<TEntity, TKey1, TKey2, TKey3> Query => GetQuery<IQuery<TEntity, TKey1, TKey2, TKey3>>();
        private ICommand<TEntity, TKey1, TKey2, TKey3> Command => GetCommand<ICommand<TEntity, TKey1, TKey2, TKey3>>();

        public Service(
            IQuery<TEntity, TKey1, TKey2, TKey3> query,
            ICommand<TEntity, TKey1, TKey2, TKey3> command)
            : base(query, command) { }

        private Service(ITransient transient)
            : base(transient,
                   Query<TEntity, TKey1, TKey2, TKey3>.Create,
                   Command<TEntity, TKey1, TKey2, TKey3>.Create)
        { }

        public static IService<TEntity, TKey1, TKey2, TKey3> Create(ITransient transient)
            => new Service<TEntity, TKey1, TKey2, TKey3>(transient);

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
        public void Delete(TKey1 key1, TKey2 key2, TKey3 key3)
            => this.Command.Delete(key1, key2, key3);

        /// <inheritdoc/>
        public void Delete(params ICompositeKeyValue<TKey1, TKey2, TKey3>[] keys)
            => this.Command.Delete(keys);

        /// <inheritdoc/>
        public void Delete(IEnumerable<ICompositeKeyValue<TKey1, TKey2, TKey3>> keys)
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
        public TEntity Get(TKey1 key1, TKey2 key2, TKey3 key3)
            => this.Query.Get(key1, key2, key3);

        /// <inheritdoc/>
        public Task<TEntity> GetAsync(TKey1 key1, TKey2 key2, TKey3 key3, CancellationToken cancellationToken = default)
            => this.Query.GetAsync(key1, key2, key3, cancellationToken);

        /// <inheritdoc/>
        public IEnumerable<TEntity> Get(params ICompositeKeyValue<TKey1, TKey2, TKey3>[] keys)
            => this.Query.Get(keys);

        /// <inheritdoc/>
        public Task<IEnumerable<TEntity>> GetAsync(ICompositeKeyValue<TKey1, TKey2, TKey3>[] keys, CancellationToken cancellationToken = default)
            =>this.Query.GetAsync(keys, cancellationToken);

        /// <inheritdoc/>
        public IEnumerable<TEntity> Get(IEnumerable<ICompositeKeyValue<TKey1, TKey2, TKey3>> keys)
            => this.Query.Get(keys);

        /// <inheritdoc/>
        public Task<IEnumerable<TEntity>> GetAsync(IEnumerable<ICompositeKeyValue<TKey1, TKey2, TKey3>> keys, CancellationToken cancellationToken = default)
            => this.Query.GetAsync(keys, cancellationToken);

        /// <inheritdoc/>
        public void Update(dynamic toUpdate, TKey1 key1, TKey2 key2, TKey3 key3)
            => this.Command.Update(toUpdate, key1, key2, key3);

        /// <inheritdoc/>
        public void Update(TEntity toUpdate, Expression<Func<TEntity, bool>> expression)
            => this.Command.Update(toUpdate, expression);

        /// <inheritdoc/>
        public void Update(dynamic toUpdate, Expression<Func<TEntity, bool>> expression)
            => this.Command.Update(toUpdate, expression);

        /// <inheritdoc/>
        public void Update(TEntity toUpdate)
            => this.Command.Update(toUpdate);

        /// <inheritdoc/>
        public async Task UpdateAsync(TEntity toUpdate, CancellationToken cancellationToken = default)
            => await this.Command.UpdateAsync(toUpdate, cancellationToken);

        /// <inheritdoc/>
        public async Task UpdateAsync(dynamic toUpdate, TKey1 key1, TKey2 key2, TKey3 key3, CancellationToken cancellationToken = default)
            => await this.Command.UpdateAsync(toUpdate, key1, key2, key3, cancellationToken);

        /// <inheritdoc/>
        public async Task DeleteAsync(TKey1 key1, TKey2 key2, TKey3 key3, CancellationToken cancellationToken = default)
            => await this.Command.DeleteAsync(key1, key2, key3, cancellationToken);

        /// <inheritdoc/>
        public async Task DeleteAsync(ICompositeKeyValue<TKey1, TKey2, TKey3>[] keys, CancellationToken cancellationToken = default)
            => await this.Command.DeleteAsync(keys, cancellationToken);

        /// <inheritdoc/>
        public async Task DeleteAsync(IEnumerable<ICompositeKeyValue<TKey1, TKey2, TKey3>> keys, CancellationToken cancellationToken = default)
            => await this.Command.DeleteAsync(keys, cancellationToken);

        /// <inheritdoc/>
        public async Task<TEntity> AddAsync(TEntity toAdd, CancellationToken cancellationToken = default)
            => await this.Command.AddAsync(toAdd, cancellationToken);

        /// <inheritdoc/>
        public async Task<IEnumerable<TEntity>> AddAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken = default)
            => await this.Command.AddAsync(items, cancellationToken);

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

