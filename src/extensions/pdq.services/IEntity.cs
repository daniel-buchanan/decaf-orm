using System;
namespace pdq.services
{
    /// <summary>
    /// An Entity.
    /// </summary>
	public interface IEntity { }

    /// <summary>
    /// An Entity with a single Primary Key.
    /// </summary>
    /// <typeparam name="TKey">The data type of the Key.</typeparam>
	public interface IEntity<TKey> : IEntity
    {
        /// <summary>
        /// Metadata about the Primary Key for this <see cref="IEntity{TKey}"/>
        /// </summary>
        IKeyMetadata KeyMetadata { get; }
    }

    /// <summary>
    /// An Entity with a composite Primary Key.
    /// </summary>
    /// <typeparam name="TKey1">The data type for the first component of the key.</typeparam>
    /// <typeparam name="TKey2">The data type for the second component of the key.</typeparam>
    public interface IEntity<TKey1, TKey2> : IEntity
    {
        /// <summary>
        /// Metadata about the Primary Key for this <see cref="IEntity{TKey1, TKey2}"/>
        /// </summary>
        ICompositeKey KeyMetadata { get; }
    }

    /// <summary>
    /// An Entity with a composite Primary Key.
    /// </summary>
    /// <typeparam name="TKey1">The data type for the first component of the key.</typeparam>
    /// <typeparam name="TKey2">The data type for the second component of the key.</typeparam>
    /// <typeparam name="TKey3">The data type for the third component of the key.</typeparam>
    public interface IEntity<TKey1, TKey2, TKey3> : IEntity
    {
        /// <summary>
        /// Metadata about the Primary Key for this <see cref="IEntity{TKey1, TKey2, TKey3}"/>
        /// </summary>
        ICompositeKeyTriple KeyMetadata { get; }
    }
}

