using System;
using System.Data;
using System.Data.Common;

namespace decaf.tests.common.Mocks;

public class MockDbConnection : DbConnection
{
    private ConnectionState state;
    private readonly MockDatabaseOptions options;

    public MockDbConnection(MockDatabaseOptions options)
    {
        this.options = options;
        state = ConnectionState.Closed;
    }

    public override string ConnectionString { get; set; }

    public override int ConnectionTimeout => 0;

    public override string Database => String.Empty;

    public override ConnectionState State => state;

    public override string DataSource { get; }

    public override string ServerVersion { get; }

    public override void ChangeDatabase(string databaseName) { }

    public override void Close() => state = ConnectionState.Closed;

    public override void Open() => state = ConnectionState.Open;

    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        => new MockDbTransaction(options, this, isolationLevel);

    protected override DbCommand CreateDbCommand() => new MockDbCommand();
}