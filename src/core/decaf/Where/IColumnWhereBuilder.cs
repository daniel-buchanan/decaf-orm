namespace decaf;

public interface IColumnWhereBuilder
{
    IColumnValueBuilder Is();
    IColumnValueBuilder IsNot();
}