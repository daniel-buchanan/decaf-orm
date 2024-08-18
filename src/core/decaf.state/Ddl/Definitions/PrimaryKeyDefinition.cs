using System.Collections.Generic;
using System.Text;

namespace decaf.state.Ddl.Definitions;

public class PrimaryKeyDefinition : IPrimaryKeyDefinition
{
    protected PrimaryKeyDefinition(string name, params IColumnDefinition[] columns)
    {
        Columns = columns;
        Name = name;
        if (string.IsNullOrWhiteSpace(name)) Name = GenerateName(name, columns);
    }

    private string GenerateName(string type, IEnumerable<IColumnDefinition> columns)
    {
        var sb = new StringBuilder("pk_");
        sb.Append(type);
        foreach (var c in columns)
            sb.AppendFormat("_{0}", c.Name);
        return sb.ToString();
    }

    public static IPrimaryKeyDefinition Create<T>(params IColumnDefinition[] columns)
        => new PrimaryKeyDefinition<T>(columns);

    public static IPrimaryKeyDefinition Create(string name, params IColumnDefinition[] columns)
        => new PrimaryKeyDefinition(name, columns);
    
    /// <inheritdoc />
    public string Name { get; }
    
    /// <inheritdoc />
    public IEnumerable<IColumnDefinition> Columns { get; }
}

public class PrimaryKeyDefinition<T>(params IColumnDefinition[] columns)
    : PrimaryKeyDefinition("pk_" + typeof(T).Name, columns), IPrimaryKeyDefinition<T>;