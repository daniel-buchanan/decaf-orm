using System;
namespace pdq.services
{
    public class CompositeKey<TKey1, TKey2> :
        ICompositeKey
    {
        public IKeyMetadata ComponentOne { get; set; }
        public IKeyMetadata ComponentTwo { get; set; }
    }

    public class CompositeKey<TKey1, TKey2, TKey3>
        : CompositeKey<TKey1, TKey2>,
        ICompositeKeyTriple
    {
        public IKeyMetadata ComponentThree { get; set; }
    }
}

