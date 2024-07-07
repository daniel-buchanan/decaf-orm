using System.Collections.Generic;

namespace decaf.services
{
    /// <summary>
    /// A Service for making modifications to a given <see cref="TEntity"/>. 
    /// </summary>
    /// <typeparam name="TEntity">The type of <see cref="IEntity{TKey1, TKey2}"/> to work with.</typeparam>
    /// <typeparam name="TKey">The data type of the primary key.</typeparam>
	public partial interface ICommand<TEntity, TKey1, TKey2> :
        ICommand<TEntity>
        where TEntity : IEntity<TKey1, TKey2>, new()
    {
        /// <summary>
        /// Updated an entire entity, using the values of the keys to restrict it.
        /// </summary>
        /// <param name="toUpdate">The entity to update.</param>
        void Update(TEntity toUpdate);

        /// <summary>
        /// Update only select fields for a specific item.
        /// </summary>
        /// <param name="toUpdate">The fields/columns to update.</param>
        /// <param name="key1">The <see cref="TKey"/> of the item to update.</param>
        /// <param name="key2">The <see cref="TKey"/> of the item to update.</param>
		void Update(dynamic toUpdate, TKey1 key1, TKey2 key2);

        /// <summary>
        /// Delete a specific item.
        /// </summary>
        /// <param name="key1">The <see cref="TKey"/> of the item to delete.</param>
        /// <param name="key2">The <see cref="TKey"/> of the item to delete.</param>
        void Delete(TKey1 key1, TKey2 key2);

        /// <summary>
        /// Delete a number of specified items.
        /// </summary>
        /// <param name="keys">The set of <see cref="TKey"/>s to delete. </param>
		void Delete(params ICompositeKeyValue<TKey1, TKey2>[] keys);

        /// <summary>
        /// Delete a number of specified items.
        /// </summary>
        /// <param name="keys">The set of <see cref="TKey"/>s to delete. </param>
		void Delete(IEnumerable<ICompositeKeyValue<TKey1, TKey2>> keys);
    }
}

