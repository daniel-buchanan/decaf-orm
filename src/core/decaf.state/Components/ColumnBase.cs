using decaf.common;

namespace decaf.state;

public abstract class ColumnBase<T>
    where T : class
{
    protected ColumnBase(string name, IQueryTarget source)
    {
        Name = name;
        Source = source;
    }

    public string Name { get; private set; }

    public IQueryTarget Source { get; private set; }

    public abstract bool IsEquivalentTo(T column);
}