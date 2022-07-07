using System;
namespace pdq.services
{
    public class CompositeKey<TKey1, TKey2> :
        ICompositeKey<TKey1, TKey2>
    {
        public IKeyMetadata<TKey1> ComponentOne { get; set; }
        public IKeyMetadata<TKey2> ComponentTwo { get; set; }
    }

    public class CompositeKey<TKey1, TKey2, TKey3>
        : CompositeKey<TKey1, TKey2>,
        ICompositeKey<TKey1, TKey2, TKey3>
    {
        public IKeyMetadata<TKey3> ComponentThree { get; set; }
    }
}

