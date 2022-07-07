using System;
namespace pdq.services
{
    public static class CompositeKeyValue
    {
        public static ICompositeKeyValue<TKey1, TKey2> Create<TKey1, TKey2>(
            TKey1 component1,
            TKey2 component2)
        {
            return new CompositeKeyValue<TKey1, TKey2>(component1, component2);
        }

        public static ICompositeKeyValue<TKey1, TKey2, TKey3> Create<TKey1, TKey2, TKey3>(
            TKey1 component1,
            TKey2 component2,
            TKey3 component3)
        {
            return new CompositeKeyValue<TKey1, TKey2, TKey3>(component1, component2, component3);
        }
    }

    public class CompositeKeyValue<TKey1, TKey2> : ICompositeKeyValue<TKey1, TKey2>
    {
        public CompositeKeyValue(TKey1 component1, TKey2 component2)
        {
            ComponentOne = component1;
            ComponentTwo = component2;
        }

        public TKey1 ComponentOne { get; }

        public TKey2 ComponentTwo { get; }
    }

    public class CompositeKeyValue<TKey1, TKey2, TKey3>
        : CompositeKeyValue<TKey1, TKey2>,
        ICompositeKeyValue<TKey1, TKey2, TKey3>
    {
        public CompositeKeyValue(TKey1 component1, TKey2 component2, TKey3 component3)
            : base(component1, component2)
        {
            ComponentThree = component3;
        }

        public TKey3 ComponentThree { get; }
    }
}

