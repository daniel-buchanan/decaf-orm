using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using pdq.common.Connections;
using pdq.common.Utilities;

namespace pdq.services
{
    internal class Service<TEntity, TKey1, TKey2, TKey3> :
        ServiceBase<TEntity>,
        IService<TEntity, TKey1, TKey2, TKey3>
        where TEntity : class, IEntity<TKey1, TKey2, TKey3>, new()
    {
        private IQuery<TEntity, TKey1, TKey2, TKey3> Query => GetQuery<IQuery<TEntity, TKey1, TKey2, TKey3>>();
        private ICommand<TEntity, TKey1, TKey2, TKey3> Command => GetCommand<ICommand<TEntity, TKey1, TKey2, TKey3>>();

        public Service(
            IQuery<TEntity, TKey1, TKey2, TKey3> query,
            ICommand<TEntity, TKey1, TKey2, TKey3> command)
            : base(query, command) { }

        private Service(IUnitOfWork unitOfWork)
            : base(unitOfWork,
                   Query<TEntity, TKey1, TKey2, TKey3>.Create,
                   Command<TEntity, TKey1, TKey2, TKey3>.Create)
        { }

        public static IService<TEntity, TKey1, TKey2, TKey3> Create(IUnitOfWork unitOfWork)
            => new Service<TEntity, TKey1, TKey2, TKey3>(unitOfWork);

        /// <inheritdoc/>
        public void Delete(TKey1 key1, TKey2 key2, TKey3 key3)
            => DeleteAsync(key1, key2, key3).WaitFor();

        /// <inheritdoc/>
        public void Delete(params ICompositeKeyValue<TKey1, TKey2, TKey3>[] keys)
            => DeleteAsync(keys?.AsEnumerable()).WaitFor();

        /// <inheritdoc/>
        public void Delete(IEnumerable<ICompositeKeyValue<TKey1, TKey2, TKey3>> keys)
            => DeleteAsync(keys).WaitFor();
        
        /// <inheritdoc/>
        public TEntity Get(TKey1 key1, TKey2 key2, TKey3 key3)
            => GetAsync(key1, key2, key3).WaitFor();

        /// <inheritdoc/>
        public IEnumerable<TEntity> Get(params ICompositeKeyValue<TKey1, TKey2, TKey3>[] keys)
            => GetAsync(keys?.AsEnumerable()).WaitFor();

        /// <inheritdoc/>
        public IEnumerable<TEntity> Get(IEnumerable<ICompositeKeyValue<TKey1, TKey2, TKey3>> keys)
            => GetAsync(keys).WaitFor();

        /// <inheritdoc/>
        public Task<TEntity> GetAsync(TKey1 key1, TKey2 key2, TKey3 key3, CancellationToken cancellationToken = default)
            => this.Query.GetAsync(key1, key2, key3, cancellationToken);

        /// <inheritdoc/>
        public Task<IEnumerable<TEntity>> GetAsync(ICompositeKeyValue<TKey1, TKey2, TKey3>[] keys, CancellationToken cancellationToken = default)
            => this.Query.GetAsync(keys, cancellationToken);

        /// <inheritdoc/>
        public Task<IEnumerable<TEntity>> GetAsync(IEnumerable<ICompositeKeyValue<TKey1, TKey2, TKey3>> keys, CancellationToken cancellationToken = default)
            => this.Query.GetAsync(keys, cancellationToken);

        /// <inheritdoc/>
        public void Update(dynamic toUpdate, TKey1 key1, TKey2 key2, TKey3 key3)
        {
            var t = UpdateAsync(toUpdate, key1, key2, key3);
            t.Wait();
        }
        
        /// <inheritdoc/>
        public void Update(TEntity toUpdate)
            => UpdateAsync(toUpdate).WaitFor();

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
    }
}

