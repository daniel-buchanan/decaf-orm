using System.Threading.Tasks;
using pdq.common.Connections;
using pdq.common.Exceptions;

namespace pdq.tests.common.Mocks
{
	public class MockConnectionDetails : ConnectionDetails
	{
        protected override int DefaultPort => 0;

        protected override string HostRegex => ".+";

        protected override string PortRegex => ".+";

        protected override string DatabaseRegex => ".+";

        protected override Task<string> ConstructConnectionStringAsync()
        {
            var connStr = $"{Hostname}:{Port},{DatabaseName}";
            return Task.FromResult(connStr);
        }

        protected override ConnectionStringParsingException ValidateConnectionString(string connectionString)
            => null;
    }
}

