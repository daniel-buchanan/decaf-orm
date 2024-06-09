using System.Collections.Generic;

namespace decaf.state.DDL;

public interface IPrimaryKeyDefinition
{
    /// <summary>
    /// The name of this Primary Key.
    /// </summary>
    string Name { get; }
    
    /// <summary>
    /// The columns that define this Primary Key.
    /// </summary>
    IEnumerable<IColumnDefinition> Columns { get; }
}