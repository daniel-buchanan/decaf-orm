using System;

namespace pdq.services
{
    /// <summary>
    /// Meta data about a Key for an <see cref="IEntity"/> or <see cref="IEntity{TKey}"/>.
    /// </summary>
    /// <typeparam name="T">The datatype of the key.</typeparam>
    public interface IKeyMetadata<T>
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