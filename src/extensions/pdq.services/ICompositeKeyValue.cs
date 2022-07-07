namespace pdq.services
{
    public interface ICompositeKeyValue<TKey1, TKey2>
    {
        TKey1 ComponentOne { get; }
        TKey2 ComponentTwo { get; }
    }

    public interface ICompositeKeyValue<TKey1, TKey2, TKey3>
        : ICompositeKeyValue<TKey1, TKey2>
    {
        TKey3 ComponentThree { get; }
    }
}