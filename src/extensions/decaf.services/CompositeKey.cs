namespace decaf.services;

public class CompositeKey :
    ICompositeKey
{
    public IKeyMetadata ComponentOne { get; set; }
    public IKeyMetadata ComponentTwo { get; set; }
}

public class CompositeKeyTriple
    : CompositeKey,
        ICompositeKeyTriple
{
    public IKeyMetadata ComponentThree { get; set; }
}