namespace decaf;

public interface IColumnMatchBuilder
{
    void Column(string name, string targetAlias = null);
}