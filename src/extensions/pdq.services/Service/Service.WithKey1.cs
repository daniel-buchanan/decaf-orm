using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using pdq.common;
using pdq.common.Connections;
using pdq.common.Utilities;

namespace pdq.services
{
    internal class Service<TEntity, TKey> :
        ServiceBase<TEntity>,
        IService<TEntity, TKey>
        where TEntity : class, IEntity<TKey>, new()
    {
        private IQuery<TEntity, TKey> Query => GetQuery<IQuery<TEntity, TKey>>();
        private ICommand<TEntity, TKey> Command => GetCommand<ICommand<TEntity, TKey>>();

        public Service(
            ISqlFactory sqlFactory,
            IQuery<TEntity, TKey> query,
            ICommand<TEntity, TKey> command)
            : base(sqlFactory, query, command) { }

        private Service(
            IUnitOfWork unitOfWork)
            : base(
                unitOfWork,
                (unitOfWork as IUnitOfWorkExtended)?.SqlFactory,
                Query<TEntity, TKey>.Create,
                Command<TEntity, TKey>.Create) { }

        public static IService<TEntity, TKey> Create(
            IUnitOfWork unitOfWork)
            => new Service<TEntity, TKey>(unitOfWork);

        /// <inheritdoc/>
        public void Delete(TKey key)
            => DeleteAsync(key).WaitFor();

        /// <inheritdoc/>
        public void Delete(params TKey[] keys)
            => DeleteAsync(keys?.AsEnumerable()).WaitFor();

        /// <inheritdoc/>
        public void Delete(IEnumerable<TKey> keys)
            => DeleteAsync(keys).WaitFor();

        /// <inheritdoc/>
        public TEntity Get(TKey key)
            => GetAsync(key).WaitFor();

        /// <inheritdoc/>
        public IEnumerable<TEntity> Get(params TKey[] keys)
            => GetAsync(keys?.AsEnumerable()).WaitFor();

        /// <inheritdoc/>
        public IEnumerable<TEntity> Get(IEnumerable<TKey> keys)
            => GetAsync(keys).WaitFor();

        /// <inheritdoc/>
        public Task<TEntity> GetAsync(TKey key, CancellationToken cancellationToken = default)
            => this.Query.GetAsync(key, cancellationToken);

        /// <inheritdoc/>
        public Task<IEnumerable<TEntity>> GetAsync(TKey[] keys, CancellationToken cancellationToken = default)
            => this.Query.GetAsync(keys, cancellationToken);

        /// <inheritdoc/>
        public Task<IEnumerable<TEntity>> GetAsync(IEnumerable<TKey> keys, CancellationToken cancellationToken = default)
            => this.Query.GetAsync(keys, cancellationToken);

        /// <inheritdoc/>
        public void Update(dynamic toUpdate, TKey key)
        {
            var t = UpdateAsync(toUpdate, key);
            t.Wait();
        }

        /// <inheritdoc/>
        public void Update(TEntity item)
            => UpdateAsync(item).WaitFor();

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
    }
}

