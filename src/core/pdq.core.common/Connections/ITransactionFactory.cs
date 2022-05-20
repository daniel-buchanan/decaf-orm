using System;
using System.Threading.Tasks;

namespace pdq.core.common.Connections
{
	public interface ITransactionFactory
	{
		ITransaction Get(IConnectionDetails connection);

		Task<ITransaction> GetAsync(IConnectionDetails connection);
	}
}

