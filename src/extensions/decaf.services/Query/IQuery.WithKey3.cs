using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace decaf.services;

/// <summary>
/// Service for querying a given Entity with a Primary Key.
/// </summary>
/// <typeparam name="TEntity">The type of <see cref="IEntity{TKey}"/> to query.</typeparam>
/// <typeparam name="TKey1">The first data type of the composite Primary Key.</typeparam>
/// <typeparam name="TKey2">The second data type of the composite Primary Key.</typeparam>
/// <typeparam name="TKey3">The third data type of the composite Primary Key.</typeparam>
public interface IQuery<TEntity, in TKey1, in TKey2, in TKey3> : IQuery<TEntity>
	where TEntity : IEntity<TKey1, TKey2, TKey3>
{
	/// <summary>
	/// Get a specific Entity by it's Key.
	/// </summary>
	/// <param name="key1">The value of the first key to use.</param>
	/// <param name="key2">The value of the second key to use.</param>
	/// <param name="key3">The value of the third key to use.</param>
	/// <returns>An instance of <see cref="TEntity"/> if found, or <code>default(TEntity)</code> if not found.</returns>
	TEntity Get(TKey1 key1, TKey2 key2, TKey3 key3);

	/// <summary>
	/// Get Entities by their Keys.
	/// </summary>
	/// <param name="keys">The keys to use.</param>
	/// <returns>An enumeration containing all found items.</returns>
	IEnumerable<TEntity> Get(params ICompositeKeyValue<TKey1, TKey2, TKey3>[] keys);

	/// <summary>
	/// Get Entities by their Keys.
	/// </summary>
	/// <param name="keys">The keys to use.</param>
	/// <returns>An enumerable containing all found items.</returns>
	IEnumerable<TEntity> Get(IEnumerable<ICompositeKeyValue<TKey1, TKey2, TKey3>> keys);

	/// <summary>
	/// Get a specific Entity by it's Key.
	/// </summary>
	/// <param name="key1">The value of the first key to use.</param>
	/// <param name="key2">The value of the second key to use.</param>
	/// <param name="key3">The value of the third key to use.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>An instance of <see cref="TEntity"/> if found, or <code>default(TEntity)</code> if not found.</returns>
	Task<TEntity> GetAsync(TKey1 key1, TKey2 key2, TKey3 key3, CancellationToken cancellationToken = default);

	/// <summary>
	/// Get Entities by their Keys.
	/// </summary>
	/// <param name="keys">The keys to use.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>An enumeration containing all found items.</returns>
	Task<IEnumerable<TEntity>> GetAsync(ICompositeKeyValue<TKey1, TKey2, TKey3>[] keys, CancellationToken cancellationToken = default);

	/// <summary>
	/// Get Entities by their Keys.
	/// </summary>
	/// <param name="keys">The keys to use.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>An enumerable containing all found items.</returns>
	Task<IEnumerable<TEntity>> GetAsync(IEnumerable<ICompositeKeyValue<TKey1, TKey2, TKey3>> keys, CancellationToken cancellationToken = default);
}