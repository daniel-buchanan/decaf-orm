using System;
namespace pdq.services
{
    public static class KeyMetadata
    {
        /// <summary>
        /// Create a new instance of <see cref="KeyMetadata{T}"/>.
        /// </summary>
        /// <param name="name">The name of the Key</param>
        /// <returns>A new instance of <see cref="KeyMetadata{T}"/> which implements <see cref="IKeyMetadata"/>.</returns>
        public static IKeyMetadata Create<T>(string name)
            => KeyMetadata<T>.Create(name);
    }

    public class KeyMetadata<T> : IKeyMetadata
    {
        private KeyMetadata(string name) => this.Name = name;

        public KeyMetadata() { }

        /// <summary>
        /// Create a new instance of <see cref="KeyMetadata{T}"/>.
        /// </summary>
        /// <param name="name">The name of the Key</param>
        /// <returns>A new instance of <see cref="KeyMetadata{T}"/> which implements <see cref="IKeyMetadata"/>.</returns>
        public static IKeyMetadata Create(string name) => new KeyMetadata<T>(name);

        /// <inheritdoc/>
        public Type Type => typeof(T);

        /// <inheritdoc/>
        public string Name { get; set; }
    }
}

