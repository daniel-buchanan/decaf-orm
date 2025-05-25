using System.Collections.Generic;

namespace decaf.state.Ddl.Definitions;

public interface IPrimaryKeyDefinition
{
    /// <summary>
    /// The name of this Primary Key.
    /// </summary>
    string Name { get; }
    
    /// <summary>
    /// The table this Primary Key is for.
    /// </summary>
    string Table { get; }
    
    /// <summary>
    /// The columns that define this Primary Key.
    /// </summary>
    IEnumerable<IColumnDefinition> Columns { get; }
}