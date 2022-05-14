using System;
using System.Threading.Tasks;

namespace pdq.core.Connections
{
	public interface IConnectionFactory : IDisposable, IAsyncDisposable
	{
		IConnection Get(IConnectionDetails connectionDetails);

		Task<IConnection> GetAsync(IConnectionDetails connectionDetails);
	}
}

