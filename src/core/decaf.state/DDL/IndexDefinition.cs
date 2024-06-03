using System.Collections.Generic;

namespace decaf.state.DDL;

public class IndexDefinition : IIndexDefinition
{
    private IndexDefinition(params IColumnDefinition[] columns) => Columns = columns;

    private IndexDefinition(string name, params IColumnDefinition[] columns) : this(columns) => Name = name;

    public static IIndexDefinition Create(params IColumnDefinition[] columns)
        => new IndexDefinition(columns);

    public static IIndexDefinition Create(string name, params IColumnDefinition[] columns)
        => new IndexDefinition(name, columns);
    
    /// <inheritdoc />
    public string Name { get; }
    
    /// <inheritdoc />
    public IEnumerable<IColumnDefinition> Columns { get; }
}