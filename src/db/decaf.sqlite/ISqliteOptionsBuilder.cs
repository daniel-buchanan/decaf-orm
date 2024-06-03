using decaf.common.Options;
using decaf.db.common;

namespace decaf.sqlite;

public interface ISqliteOptionsBuilder : 
    ISqlOptionsBuilder<SqliteOptions, ISqliteOptionsBuilder, ISqliteConnectionDetails>
{
    /// <summary>
    /// The local file path for the database.
    /// Note that if <see cref="InMemory" /> is set to true, then this will default to ":memory:".
    /// </summary>
    /// <param name="path">The file path for the database</param>
    /// <returns></returns>
    ISqliteOptionsBuilder WithFilePath(string path);

    /// <summary>
    /// Whether or not the database is in memory.
    /// This will overwrite anything provided to <see cref="WithFilePath"/>.
    /// </summary>
    /// <returns></returns>
    ISqliteOptionsBuilder InMemory();

    /// <summary>
    /// Connect as read only
    /// </summary>
    /// <returns></returns>
    ISqliteOptionsBuilder AsReadonly();

    /// <summary>
    /// Whether or not a new database should be created.
    /// Note that if <see cref="InMemory"/> is set to true, this will be overwritten to true.
    /// </summary>
    /// <returns></returns>
    ISqliteOptionsBuilder CreateNewDatabase();
}