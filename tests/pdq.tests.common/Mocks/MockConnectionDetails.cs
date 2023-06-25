using System;
using System.Threading.Tasks;
using pdq.common.Connections;

namespace pdq.tests.common.Mocks
{
	public class MockConnectionDetails : ConnectionDetails
	{
        protected override int DefaultPort => 0;

        protected override string HostPortRegex => ".+";

        protected override string DatabaseRegex => ".+";

        protected override Task<string> ConstructConnectionStringAsync()
        {
            var connStr = $"{Hostname}:{Port},{DatabaseName}";
            return Task.FromResult(connStr);
        }
    }
}

