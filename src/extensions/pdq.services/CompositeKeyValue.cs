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

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            var incoming = obj as ICompositeKeyValue<TKey1, TKey2>;
            if (incoming == null) return false;
            return ComponentOne.Equals(incoming.ComponentOne) &&
                ComponentTwo.Equals(incoming.ComponentTwo);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hash1 = ComponentOne.GetHashCode();
            var hash2 = ComponentTwo.GetHashCode();
            return (hash1 + hash2) / 2;
        }

        /// <inheritdoc/>
        public override string ToString()
            => $"({ComponentOne},{ComponentTwo})";
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

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            var incoming = obj as ICompositeKeyValue<TKey1, TKey2, TKey3>;
            if (incoming == null) return false;
            return ComponentOne.Equals(incoming.ComponentOne) &&
                ComponentTwo.Equals(incoming.ComponentTwo) &&
                ComponentThree.Equals(incoming.ComponentThree);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hash1 = ComponentOne.GetHashCode();
            var hash2 = ComponentTwo.GetHashCode();
            var hash3 = ComponentThree.GetHashCode();
            return (hash1 + hash2 + hash3) / 2;
        }

        /// <inheritdoc/>
        public override string ToString()
            => $"({ComponentOne},{ComponentTwo},{ComponentThree})";
    }
}

