using System;
using System.Threading.Tasks;

namespace pdq.core.Connections
{
	public interface ITransactionFactory
	{
		ITransaction Get(IConnectionDetails connection);

		Task<ITransaction> GetAsync(IConnectionDetails connection);
	}
}

