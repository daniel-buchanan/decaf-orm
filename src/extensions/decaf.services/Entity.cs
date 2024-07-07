using decaf.common.Attributes;
using decaf.common.Utilities.Reflection;

namespace decaf.services
{
    public abstract class Entity : IEntity { }

    public abstract class Entity<TKey> : IEntity<TKey>
    {
        protected Entity(string keyName)
        {
            KeyMetadata = services.KeyMetadata.Create<TKey>(keyName);
        }

        /// <inheritdoc/>
        [IgnoreColumnFor.All]
        public IKeyMetadata KeyMetadata { get; }

        /// <inheritdoc/>
        public TKey GetKeyValue()
            => (TKey)this.GetPropertyValue(KeyMetadata.Name);
    }

    public abstract class Entity<TKey1, TKey2> : IEntity<TKey1, TKey2>
    {
        protected Entity(string componentOne, string componentTwo)
        {
            KeyMetadata = new CompositeKey
            {
                ComponentOne = services.KeyMetadata.Create<TKey1>(componentOne),
                ComponentTwo = services.KeyMetadata.Create<TKey2>(componentTwo)
            };
        }

        /// <inheritdoc/>
        [IgnoreColumnFor.All]
        public ICompositeKey KeyMetadata { get; }

        /// <inheritdoc/>
        public ICompositeKeyValue<TKey1, TKey2> GetKeyValue()
        {
            var value1 = (TKey1)this.GetPropertyValue(KeyMetadata.ComponentOne.Name);
            var value2 = (TKey2)this.GetPropertyValue(KeyMetadata.ComponentTwo.Name);
            return CompositeKeyValue.Create(value1, value2);
        }
    }

    public abstract class Entity<TKey1, TKey2, TKey3> : IEntity<TKey1, TKey2, TKey3>
    {
        protected Entity(string componentOne, string componentTwo, string componentThree)
        {
            KeyMetadata = new CompositeKeyTriple
            {
                ComponentOne = services.KeyMetadata.Create<TKey1>(componentOne),
                ComponentTwo = services.KeyMetadata.Create<TKey2>(componentTwo),
                ComponentThree = services.KeyMetadata.Create<TKey3>(componentThree)
            };
        }

        /// <inheritdoc/>
        [IgnoreColumnFor.All]
        public ICompositeKeyTriple KeyMetadata { get; }

        /// <inheritdoc/>
        public ICompositeKeyValue<TKey1, TKey2, TKey3> GetKeyValue()
        {
            var value1 = (TKey1)this.GetPropertyValue(KeyMetadata.ComponentOne.Name);
            var value2 = (TKey2)this.GetPropertyValue(KeyMetadata.ComponentTwo.Name);
            var value3 = (TKey3)this.GetPropertyValue(KeyMetadata.ComponentThree.Name);
            return CompositeKeyValue.Create(value1, value2, value3);
        }
    }
}

