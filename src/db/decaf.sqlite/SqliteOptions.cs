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

    public string? DatabasePath { get; private set; }
    
    public bool InMemory { get; private set; }
    
    public bool CreateNew { get; private set; }

    public bool ReadOnly { get; private set; }
    
    public bool ConstructConnectionFromOptions { get; private set; }
}