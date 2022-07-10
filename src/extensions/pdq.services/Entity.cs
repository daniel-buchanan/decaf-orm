using System;
using pdq.Attributes;

namespace pdq.services
{
    public class Entity : IEntity { }

    public abstract class Entity<TKey> : IEntity<TKey>
    {
        protected Entity(string keyName)
        {
            KeyMetadata = KeyMetadata<TKey>.Create(keyName);
        }

        [IgnoreColumnFor.All]
        public IKeyMetadata KeyMetadata { get; }
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

        [IgnoreColumnFor.All]
        public ICompositeKey KeyMetadata { get; }
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

        [IgnoreColumnFor.All]
        public ICompositeKeyTriple KeyMetadata { get; }
    }
}

