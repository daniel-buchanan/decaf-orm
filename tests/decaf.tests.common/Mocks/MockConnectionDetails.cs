using System.Threading;
using System.Threading.Tasks;
using decaf.common.Connections;
using decaf.common.Exceptions;

namespace decaf.tests.common.Mocks;

public class MockConnectionDetails : ConnectionDetails
{
    protected override int DefaultPort => 0;

    protected override string HostRegex => ".+";

    protected override string PortRegex => ".+";

    protected override string DatabaseRegex => ".+";

    protected override Task<string> ConstructConnectionStringAsync(CancellationToken cancellationToken = default)
    {
        var connStr = $"{Hostname}:{Port},{DatabaseName}";
        return Task.FromResult(connStr);
    }

    protected override ConnectionStringParsingException ValidateConnectionString(string connectionString)
        => null;
}