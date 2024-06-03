using System;

namespace decaf.state.DDL;

public class ColumnDefinition : IColumnDefinition
{
    public static IColumnDefinition Create(string name, Type type, bool nullable = false)
        => new ColumnDefinition(name, type, nullable);

    private ColumnDefinition(string name, Type type, bool nullable)
    {
        Name = name;
        Type = type;
        Nullable = nullable;
    }
    
    /// <inheritdoc />
    public string Name { get; }
    
    /// <inheritdoc />
    public Type Type { get; }
    
    /// <inheritdoc />
    public bool Nullable { get; }
}