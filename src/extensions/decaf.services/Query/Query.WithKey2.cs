﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using decaf.common;
using decaf.common.Connections;
using decaf.common.Utilities;

namespace decaf.services;

internal class Query<TEntity, TKey1, TKey2> :
    Query<TEntity>,
    IQuery<TEntity, TKey1, TKey2>
    where TEntity : class, IEntity<TKey1, TKey2>, new()
{
    public Query(IDecaf decaf, ISqlFactory sqlFactory) : base(decaf, sqlFactory) { }

    private Query(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    public new static IQuery<TEntity, TKey1, TKey2> Create(IUnitOfWork unitOfWork)
        => new Query<TEntity, TKey1, TKey2>(unitOfWork);

    /// <inheritdoc/>
    public TEntity Get(TKey1 key1, TKey2 key2)
        => GetAsync(key1, key2).WaitFor();

    /// <inheritdoc/>
    public IEnumerable<TEntity> Get(params ICompositeKeyValue<TKey1, TKey2>[] keys)
        => GetAsync(keys?.AsEnumerable()).WaitFor();

    /// <inheritdoc/>
    public IEnumerable<TEntity> Get(IEnumerable<ICompositeKeyValue<TKey1, TKey2>> keys)
        => GetAsync(keys).WaitFor();

    /// <inheritdoc/>
    public async Task<TEntity> GetAsync(TKey1 key1, TKey2 key2, CancellationToken cancellationToken = default)
    {
        var results = await GetAsync(new[] { CompositeKeyValue.Create(key1, key2) }, cancellationToken);
        return results.FirstOrDefault();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TEntity>> GetAsync(ICompositeKeyValue<TKey1, TKey2>[] keys, CancellationToken cancellationToken = default)
        => await GetAsync(keys?.AsEnumerable(), cancellationToken);

    /// <inheritdoc/>
    public async Task<IEnumerable<TEntity>> GetAsync(IEnumerable<ICompositeKeyValue<TKey1, TKey2>> keys, CancellationToken cancellationToken = default)
    {
        var tmp = new TEntity();

        return await GetByKeysAsync(keys, (keyBatch, q, b) =>
        {
            var key1Name = GetKeyColumnName<TEntity>(q, tmp.KeyMetadata.ComponentOne);
            var key2Name = GetKeyColumnName<TEntity>(q, tmp.KeyMetadata.ComponentTwo);
            b.ClauseHandling.DefaultToOr();
            foreach (var k in keyBatch)
            {
                b.And(ab =>
                {
                    ab.Column(key1Name, "t").Is().EqualTo(k.ComponentOne);
                    ab.Column(key2Name, "t").Is().EqualTo(k.ComponentTwo);
                });
            }
        }, cancellationToken);
    }
}