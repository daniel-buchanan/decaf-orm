using decaf.common.Connections;
using decaf.common.Logging;

namespace decaf.sqlite;

public class SqliteConnectionFactory : ConnectionFactory
{
    private readonly SqliteOptions options;
    public SqliteConnectionFactory(ILoggerProxy logger, SqliteOptions options) : base(logger) 
        => this.options = options;

    protected override Task<IConnection> ConstructConnectionAsync(
        IConnectionDetails connectionDetails, 
        CancellationToken cancellationToken = default)
    {
        var conn = new SqliteConnection(logger, connectionDetails);
        return Task.FromResult(conn as IConnection);
    }
}