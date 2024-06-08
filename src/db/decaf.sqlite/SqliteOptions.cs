using decaf.common.Connections;
using decaf.db.common;

namespace decaf.sqlite;

public class SqliteOptions : DatabaseOptions
{
    public SqliteOptions()
    {
        ConnectionDetailsServiceProviderFactory = ConnectionDetailsFactory;
        IncludeParameterPrefix = true;
    }

    private IConnectionDetails ConnectionDetailsFactory(IServiceProvider provider)
    {
        if (ConnectionDetails != null) return ConnectionDetails;
        if (!ConstructConnectionFromOptions && ConnectionDetails != null)
            return ConnectionDetails;

        return new SqliteConnectionDetails(this);
    }

    public string? DatabasePath { get; }
    
    public bool InMemory { get; }
    
    public bool CreateNew { get; }

    public bool ReadOnly { get; }
    
    public bool ConstructConnectionFromOptions { get; }
}