using System.Collections.Generic;
using System.Text;

namespace decaf.state.Ddl.Definitions;

public class PrimaryKeyDefinition : IPrimaryKeyDefinition
{
    private PrimaryKeyDefinition(params IColumnDefinition[] columns)
    {
        Columns = columns;
        Name = GenerateName(columns);
    }

    private PrimaryKeyDefinition(string name, params IColumnDefinition[] columns)
    {
        Columns = columns;
        Name = name;
        if (string.IsNullOrWhiteSpace(name)) Name = GenerateName(columns);
    }

    private string GenerateName(IEnumerable<IColumnDefinition> columns)
    {
        var sb = new StringBuilder("pk");
        foreach (var c in columns)
            sb.AppendFormat("_{0}", c.Name);
        return sb.ToString();
    }

    public static IPrimaryKeyDefinition Create(params IColumnDefinition[] columns)
        => new PrimaryKeyDefinition(columns);

    public static IPrimaryKeyDefinition Create(string name, params IColumnDefinition[] columns)
        => new PrimaryKeyDefinition(name, columns);
    
    /// <inheritdoc />
    public string Name { get; }
    
    /// <inheritdoc />
    public IEnumerable<IColumnDefinition> Columns { get; }
}