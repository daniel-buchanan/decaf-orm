using System;
using pdq.common.Connections;

namespace pdq.playground.Mocks
{
	public class MockConnectionDetails : ConnectionDetails
	{
        protected override Task<string> ConstructConnectionString()
        {
            var connStr = $"{Hostname}:{Port},{DatabaseName}";
            return Task.FromResult(connStr);
        }
    }
}

