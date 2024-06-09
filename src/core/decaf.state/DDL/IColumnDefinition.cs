using System;

namespace decaf.state.DDL;

public interface IColumnDefinition
{
    /// <summary>
    /// The name of this Column.
    /// </summary>
    string Name { get; }
    
    /// <summary>
    /// The type of the Column.
    /// </summary>
    Type Type { get; }
    
    /// <summary>
    /// Whether or not this Column is nullable.
    /// </summary>
    bool Nullable { get; }
}