using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace decaf.state.Ddl.Definitions;

public class PrimaryKeyDefinition : IPrimaryKeyDefinition
{
    private PrimaryKeyDefinition(string table, params IColumnDefinition[] columns)
    {
        Table = table;
        Columns = columns;
        Name = GenerateName(columns);
    }
    
    private PrimaryKeyDefinition(string name, string table, params IColumnDefinition[] columns) : this(table, columns)
    {
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
    
    private string GenerateName(IEnumerable<IColumnDefinition> columns)
    {
        var sb = new StringBuilder("pk_");
        sb.AppendFormat("_{0}_", Table);
        sb.AppendFormat("{0}_", string.Join("_", columns.Select(c => c.Name)));
        return sb.ToString();
    }
}