using decaf.common;

namespace decaf.state;

public class GroupBy : ColumnBase<GroupBy>
{
    private GroupBy(string name, IQueryTarget source)
        : base(name, source)
    {
    }

    public static GroupBy Create(string name, IQueryTarget source) => new GroupBy(name, source);

    public override bool IsEquivalentTo(GroupBy column)
    {
        return column.Name == Name &&
               column.Source.Alias == Source.Alias;
    }
}