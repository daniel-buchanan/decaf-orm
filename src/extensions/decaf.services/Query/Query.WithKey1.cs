using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using decaf.common;
using decaf.common.Connections;
using decaf.common.Utilities;

namespace decaf.services;

internal class Query<TEntity, TKey> :
    Query<TEntity>,
    IQuery<TEntity, TKey>
    where TEntity : class, IEntity<TKey>, new()
{
    public Query(IDecaf decaf, ISqlFactory sqlFactory) : base(decaf, sqlFactory) { }

    protected Query(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    public new static IQuery<TEntity, TKey> Create(IUnitOfWork unitOfWork) 
        => new Query<TEntity, TKey>(unitOfWork);

    /// <inheritdoc/>
    public TEntity Get(TKey key)
        => GetAsync(key).WaitFor();

    /// <inheritdoc/>
    public IEnumerable<TEntity> Get(params TKey[] keys)
        => Get(keys.ToList());

    /// <inheritdoc/>
    public IEnumerable<TEntity> Get(IEnumerable<TKey> keys)
        => GetAsync(keys).WaitFor();

    /// <inheritdoc/>
    public async Task<TEntity> GetAsync(TKey key, CancellationToken cancellationToken = default)
    {
        var results = await GetAsync(new[] { key }, cancellationToken);
        return results.FirstOrDefault();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TEntity>> GetAsync(TKey[] keys, CancellationToken cancellationToken = default)
        => await GetAsync(keys?.ToList(), cancellationToken);

    /// <inheritdoc/>
    public Task<IEnumerable<TEntity>> GetAsync(IEnumerable<TKey> keys, CancellationToken cancellationToken = default)
    {
        var tmp = new TEntity();

        return GetByKeysAsync(keys, (keyBatch, q, b) =>
        {
            var keyName = GetKeyColumnName<TEntity>(q, tmp.KeyMetadata);
            b.Column(keyName).Is().In(keyBatch);
        }, cancellationToken);
    }
}