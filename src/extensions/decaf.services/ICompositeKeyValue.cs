namespace decaf.services
{
    public interface ICompositeKeyValue<out TKey1, out TKey2>
    {
        TKey1 ComponentOne { get; }
        TKey2 ComponentTwo { get; }
    }

    public interface ICompositeKeyValue<out TKey1, out TKey2, out TKey3>
        : ICompositeKeyValue<TKey1, TKey2>
    {
        TKey3 ComponentThree { get; }
    }
}