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
        ConfigureProperty(x => x.ReadOnly, connectionDetails.ReadOnly);
        ConfigureProperty(x => x.InMemory, connectionDetails.InMemory);
        ConfigureProperty(x => x.ConstructConnectionFromOptions, false);
        return this;
    }

    /// <inheritdoc/>
    public ISqliteOptionsBuilder WithFilePath(string path)
    {
        ConfigureProperty(x => x.DatabasePath, path);
        ConfigureProperty(x => x.ConstructConnectionFromOptions, true);
        return this;
    }

    /// <inheritdoc/>
    public ISqliteOptionsBuilder InMemory()
    {
        ConfigureProperty(x => x.InMemory, true);
        ConfigureProperty(x => x.ConstructConnectionFromOptions, true);
        return this;
    }

    /// <inheritdoc/>
    public ISqliteOptionsBuilder AsReadonly()
    {
        ConfigureProperty(x => x.ReadOnly, true);
        ConfigureProperty(x => x.ConstructConnectionFromOptions, true);
        return this;
    }

    public ISqliteOptionsBuilder CreateNewDatabase()
    {
        ConfigureProperty(x => x.CreateNew, true);
        ConfigureProperty(x => x.ConstructConnectionFromOptions, true);
        return this;
    }

    public ISqliteOptionsBuilder UseQuotedIdentifiers()
    {
        ConfigureProperty(x => x.QuotedIdentifiers, true);
        return this;
    }
}