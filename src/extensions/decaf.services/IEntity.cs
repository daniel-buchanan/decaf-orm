namespace decaf.services;

/// <summary>
/// An Entity.
/// </summary>
public interface IEntity { }

/// <summary>
/// An Entity with a single Primary Key.
/// </summary>
/// <typeparam name="TKey">The data type of the Key.</typeparam>
public interface IEntity<out TKey> : IEntity
{
    /// <summary>
    /// Metadata about the Primary Key for this <see cref="IEntity{TKey}"/>
    /// </summary>
    IKeyMetadata KeyMetadata { get; }

    /// <summary>
    /// Get the Key value for this Entity.
    /// </summary>
    /// <returns></returns>
    TKey GetKeyValue();
}

/// <summary>
/// An Entity with a composite Primary Key.
/// </summary>
/// <typeparam name="TKey1">The data type for the first component of the key.</typeparam>
/// <typeparam name="TKey2">The data type for the second component of the key.</typeparam>
public interface IEntity<out TKey1, out TKey2> : IEntity
{
    /// <summary>
    /// Metadata about the Primary Key for this <see cref="IEntity{TKey1, TKey2}"/>
    /// </summary>
    ICompositeKey KeyMetadata { get; }

    /// <summary>
    /// Get the Key value for this Entity.
    /// </summary>
    /// <returns></returns>
    ICompositeKeyValue<TKey1, TKey2> GetKeyValue();
}

/// <summary>
/// An Entity with a composite Primary Key.
/// </summary>
/// <typeparam name="TKey1">The data type for the first component of the key.</typeparam>
/// <typeparam name="TKey2">The data type for the second component of the key.</typeparam>
/// <typeparam name="TKey3">The data type for the third component of the key.</typeparam>
public interface IEntity<out TKey1, out TKey2, out TKey3> : IEntity
{
    /// <summary>
    /// Metadata about the Primary Key for this <see cref="IEntity{TKey1, TKey2, TKey3}"/>
    /// </summary>
    ICompositeKeyTriple KeyMetadata { get; }

    /// <summary>
    /// Get the Key value for this Entity.
    /// </summary>
    /// <returns></returns>
    ICompositeKeyValue<TKey1, TKey2, TKey3> GetKeyValue();
}