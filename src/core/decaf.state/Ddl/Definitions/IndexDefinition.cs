using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace decaf.state.Ddl.Definitions;

public class IndexDefinition : IIndexDefinition
{
    private IndexDefinition(string table, params IColumnDefinition[] columns) : this(string.Empty, table, columns) { }

    private IndexDefinition(string name, string table, params IColumnDefinition[] columns)
    {
        if (string.IsNullOrWhiteSpace(table))
            throw new ArgumentNullException(nameof(table));
        
        if (columns == null ||
            !columns.Any())
            throw new ArgumentNullException(nameof(columns));
        
        Columns = columns;
        Table = table;
        Name = string.IsNullOrWhiteSpace(name)
            ? GenerateName(columns)
            : name;
    }

    public static IIndexDefinition Create(string table, params IColumnDefinition[] columns)
        => new IndexDefinition(table, columns);

    public static IIndexDefinition Create(string name, string table, params IColumnDefinition[] columns)
        => new IndexDefinition(name, table, columns);

    /// <inheritdoc />
    public string Name { get; }
    
    /// <inheritdoc />
    public string Table { get; }
    
    /// <inheritdoc />
    public IEnumerable<IColumnDefinition> Columns { get; }
    
    private string GenerateName(IEnumerable<IColumnDefinition> columns)
    {
        var sb = new StringBuilder("idx_");
        sb.AppendFormat("_{0}_", Table);
        sb.AppendFormat("{0}_", string.Join("_", columns.Select(c => c.Name)));
        return sb.ToString();
    }
}