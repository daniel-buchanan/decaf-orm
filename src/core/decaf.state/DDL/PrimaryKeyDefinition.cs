using System.Collections.Generic;

namespace decaf.state.DDL;

public class PrimaryKeyDefinition : IPrimaryKeyDefinition
{
    private PrimaryKeyDefinition(params IColumnDefinition[] columns) => Columns = columns;

    private PrimaryKeyDefinition(string name, params IColumnDefinition[] columns) : this(columns) => Name = name;

    public static IPrimaryKeyDefinition Create(params IColumnDefinition[] columns)
        => new PrimaryKeyDefinition(columns);

    public static IPrimaryKeyDefinition Create(string name, params IColumnDefinition[] columns)
        => new PrimaryKeyDefinition(name, columns);
    
    /// <inheritdoc />
    public string Name { get; }
    
    /// <inheritdoc />
    public IEnumerable<IColumnDefinition> Columns { get; }
}