using System;
namespace decaf.services
{
    public static class KeyMetadata
    {
        /// <summary>
        /// Create a new instance of <see cref="KeyMetadata{T}"/>.
        /// </summary>
        /// <param name="name">The name of the Key</param>
        /// <returns>A new instance of <see cref="KeyMetadata{T}"/> which implements <see cref="IKeyMetadata"/>.</returns>
        public static IKeyMetadata Create<T>(string name)
            => new KeyMetadata<T>(name);
    }

    public class KeyMetadata<T> : IKeyMetadata
    {
        internal KeyMetadata(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), $"The {nameof(name)} argument MUST NOT be null or an empty string.");
            Name = name;
        }

        /// <inheritdoc/>
        public Type Type => typeof(T);

        /// <inheritdoc/>
        public string Name { get; set; }

        public override string ToString()
            => $"[{Type.Name}]{Name}";
    }
}

