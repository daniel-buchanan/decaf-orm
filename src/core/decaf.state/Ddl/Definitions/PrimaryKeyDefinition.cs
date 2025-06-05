using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace decaf.state.Ddl.Definitions;

public class PrimaryKeyDefinition : IPrimaryKeyDefinition
{
    private PrimaryKeyDefinition(string table, params IColumnDefinition[] columns)
    {
        if (string.IsNullOrWhiteSpace(table))
            throw new ArgumentNullException(nameof(table), "PrimaryKey must reference a table.");
        
        Table = table;
        Columns = columns;
        Name = $"pk_{table}";
    }
    
    private PrimaryKeyDefinition(string name, string table, params IColumnDefinition[] columns) : this(table, columns)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name), "PrimaryKey must have a name.");
        
        Name = name;
    }

    public static IPrimaryKeyDefinition Create(string table, params IColumnDefinition[] columns)
        => new PrimaryKeyDefinition(table, columns);
    
    public static IPrimaryKeyDefinition Create(string name, string table, params IColumnDefinition[] columns)
        => new PrimaryKeyDefinition(name, table, columns);
    
    /// <inheritdoc />
    public string Name { get; }

    public string Table { get; }

    /// <inheritdoc />
    public IEnumerable<IColumnDefinition> Columns { get; }
}