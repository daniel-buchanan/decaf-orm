using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace pdq.services
{
    /// <summary>
    /// A Service for making modifications to a given <see cref="TEntity"/>. 
    /// </summary>
    /// <typeparam name="TEntity">The type of <see cref="IEntity{TKey1, TKey2}"/> to work with.</typeparam>
    /// <typeparam name="TKey">The data type of the primary key.</typeparam>
	public partial interface ICommand<TEntity, TKey1, TKey2>
    {
        /// <summary>
        /// Updated an entire entity, using the values of the keys to restrict it.
        /// </summary>
        /// <param name="toUpdate">The entity to update.</param>
        Task UpdateAsync(TEntity toUpdate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update only select fields for a specific item.
        /// </summary>
        /// <param name="toUpdate">The fields/columns to update.</param>
        /// <param name="key1">The <see cref="TKey"/> of the item to update.</param>
        /// <param name="key2">The <see cref="TKey"/> of the item to update.</param>
		Task UpdateAsync(dynamic toUpdate, TKey1 key1, TKey2 key2, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete a specific item.
        /// </summary>
        /// <param name="key1">The <see cref="TKey"/> of the item to delete.</param>
        /// <param name="key2">The <see cref="TKey"/> of the item to delete.</param>
        Task DeleteAsync(TKey1 key1, TKey2 key2, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete a number of specified items.
        /// </summary>
        /// <param name="keys">The set of <see cref="TKey"/>s to delete. </param>
		Task DeleteAsync(ICompositeKeyValue<TKey1, TKey2>[] keys, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete a number of specified items.
        /// </summary>
        /// <param name="keys">The set of <see cref="TKey"/>s to delete. </param>
		Task DeleteAsync(IEnumerable<ICompositeKeyValue<TKey1, TKey2>> keys, CancellationToken cancellationToken = default);
    }
}

