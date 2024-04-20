using decaf.db.common;

namespace decaf.sqlite;

public class SqliteOptionsBuilder : 
    SqlOptionsBuilder<SqliteOptions, ISqliteOptionsBuilder, ISqliteConnectionDetails>,
    ISqliteOptionsBuilder
{
    /// <inheritdoc/>
    public override ISqliteOptionsBuilder WithConnectionString(string connectionString)
    {
        var connectionDetails = new SqliteConnectionDetails(connectionString);
        ConfigureProperty(x => x.ConnectionDetails, connectionDetails);
        ConfigureProperty(x => x.CreateNew, connectionDetails.CreateNew);
        ConfigureProperty(x => x.DatabasePath, connectionDetails.FullUri);
        ConfigureProperty(x => x.Version, connectionDetails.Version);
        ConfigureProperty(x => x.InMemory, connectionDetails.InMemory);
        return this;
    }

    /// <inheritdoc/>
    public ISqliteOptionsBuilder WithFilePath(string path)
    {
        ConfigureProperty(x => x.DatabasePath, path);
        return this;
    }

    /// <inheritdoc/>
    public ISqliteOptionsBuilder InMemory()
    {
        ConfigureProperty(x => x.InMemory, true);
        return this;
    }

    /// <inheritdoc/>
    public ISqliteOptionsBuilder WithVersion(decimal version)
    {
        ConfigureProperty(x => x.Version, version);
        return this;
    }

    public ISqliteOptionsBuilder CreateNewDatabase()
    {
        ConfigureProperty(x => x.CreateNew, true);
        return this;
    }

    public ISqliteOptionsBuilder UseQuotedIdentifiers()
    {
        ConfigureProperty(x => x.QuotedIdentifiers, true);
        return this;
    }
}