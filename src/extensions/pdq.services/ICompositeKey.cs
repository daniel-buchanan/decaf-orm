namespace pdq.services
{
    /// <summary>
    /// Represents metadata for a composite key which has two component keys.
    /// </summary>
    /// <typeparam name="TKey1">The data type of the first component.</typeparam>
    /// <typeparam name="TKey2">The data type of the second component.</typeparam>
    public interface ICompositeKey<TKey1, TKey2>
    {
        /// <summary>
        /// Metadata for the first component key of a composite key.
        /// </summary>
        IKeyMetadata<TKey1> ComponentOne { get; set; }

        /// <summary>
        /// Metadata for the second component key of a composite key.
        /// </summary>
        IKeyMetadata<TKey2> ComponentTwo { get; set; }
    }

    /// <summary>
    /// Represents metadata for a composite key which has three component keys.
    /// </summary>
    /// <typeparam name="TKey1">The data type of the first component.</typeparam>
    /// <typeparam name="TKey2">The data type of the second component.</typeparam>
    /// <typeparam name="TKey3">The data type of the third component.</typeparam>
    public interface ICompositeKey<TKey1, TKey2, TKey3> :
        ICompositeKey<TKey1, TKey2>
    {
        /// <summary>
        /// Metadata for the third component key of a composite key.
        /// </summary>
        IKeyMetadata<TKey3> ComponentThree { get; set; }
    }
}