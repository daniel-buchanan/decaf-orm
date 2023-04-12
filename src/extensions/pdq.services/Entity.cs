using System;
using pdq.Attributes;
using pdq.state;
using pdq.common.Utilities.Reflection;

namespace pdq.services
{
    public abstract class Entity : IEntity { }

    public abstract class Entity<TKey> : IEntity<TKey>
    {
        protected Entity(string keyName)
        {
            KeyMetadata = KeyMetadata<TKey>.Create(keyName);
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
                ComponentOne = KeyMetadata<TKey1>.Create(componentOne),
                ComponentTwo = KeyMetadata<TKey2>.Create(componentTwo)
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
                ComponentOne = KeyMetadata<TKey1>.Create(componentOne),
                ComponentTwo = KeyMetadata<TKey2>.Create(componentTwo),
                ComponentThree = KeyMetadata<TKey3>.Create(componentThree)
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

