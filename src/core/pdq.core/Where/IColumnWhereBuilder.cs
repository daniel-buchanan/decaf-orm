namespace pdq
{
    public interface IColumnWhereBuilder
    {
        IColumnValueBuilder Is();
        IColumnValueBuilder IsNot();
    }
}

