using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace pdq.services
{
    /// <summary>
    /// Service for querying a given Entity with a Primary Key.
    /// </summary>
    /// <typeparam name="TEntity">The type of <see cref="IEntity{TKey}"/> to query.</typeparam>
    /// <typeparam name="TKey">The data type of the Primary Key.</typeparam>
	public interface IQuery<TEntity, TKey> : IQuery<TEntity>
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
        /// <returns>An enumeration containg all found </returns>
		IEnumerable<TEntity> Get(params TKey[] keys);

        /// <summary>
        /// Get Entities by their Keys.
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
		IEnumerable<TEntity> Get(IEnumerable<TKey> keys);
    }
}

