namespace pdq.services
{
    /// <summary>
    /// Represents metadata for a composite key which has two component keys.
    /// </summary>
    public interface ICompositeKey
    {
        /// <summary>
        /// Metadata for the first component key of a composite key.
        /// </summary>
        IKeyMetadata ComponentOne { get; set; }

        /// <summary>
        /// Metadata for the second component key of a composite key.
        /// </summary>
        IKeyMetadata ComponentTwo { get; set; }
    }

    /// <summary>
    /// Represents metadata for a composite key which has three component keys.
    /// </summary>
    public interface ICompositeKeyTriple : ICompositeKey
    {
        /// <summary>
        /// Metadata for the third component key of a composite key.
        /// </summary>
        IKeyMetadata ComponentThree { get; set; }
    }
}