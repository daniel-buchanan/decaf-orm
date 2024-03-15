using System;

namespace decaf.services
{
    /// <summary>
    /// Meta data about a Key for an <see cref="IEntity"/> or <see cref="IEntity{TKey}"/>.
    /// </summary>
    public interface IKeyMetadata
    {
        /// <summary>
        /// The Data Type of the Key.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// The name of the Key.
        /// </summary>
        string Name { get; set; }
    }
}