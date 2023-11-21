using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace pdq.services
{
	/// <summary>
	/// Service for querying a given Entity with a Primary Key.
	/// </summary>
	/// <typeparam name="TEntity">The type of <see cref="IEntity{TKey}"/> to query.</typeparam>
	/// <typeparam name="TKey">The data type of the Primary Key.</typeparam>
	public interface IQuery<TEntity, in TKey> :
		IQuery<TEntity>
		where TEntity : IEntity<TKey>
	{
		/// <summary>
		/// Get a specific Entity by it's Key.
		/// </summary>
		/// <param name="key">The value of the key to use.</param>
		/// <returns>An instance of <see cref="TEntity"/> if found, or <code>default(TEntity)</code> if not found.</returns>
		TEntity Get(TKey key);

		/// <summary>
		/// Get Entities by their Keys.
		/// </summary>
		/// <param name="keys">The keys to use.</param>
		/// <returns>An enumeration containing all found </returns>
		IEnumerable<TEntity> Get(params TKey[] keys);

		/// <summary>
		/// Get Entities by their Keys.
		/// </summary>
		/// <param name="keys"></param>
		/// <returns></returns>
		IEnumerable<TEntity> Get(IEnumerable<TKey> keys);

		/// <summary>
		/// Get a specific Entity by it's Key.
		/// </summary>
		/// <param name="key">The value of the key to use.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An instance of <see cref="TEntity"/> if found, or <code>default(TEntity)</code> if not found.</returns>
		Task<TEntity> GetAsync(TKey key, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get Entities by their Keys.
		/// </summary>
		/// <param name="keys">The keys to use.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An enumeration containing all found </returns>
		Task<IEnumerable<TEntity>> GetAsync(TKey[] keys, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get Entities by their Keys.
		/// </summary>
		/// <param name="keys"></param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns></returns>
		Task<IEnumerable<TEntity>> GetAsync(IEnumerable<TKey> keys, CancellationToken cancellationToken = default);
	}
}

