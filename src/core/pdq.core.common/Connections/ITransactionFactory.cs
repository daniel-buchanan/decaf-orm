using System;
using System.Threading.Tasks;

namespace pdq.common.Connections
{
	public interface ITransactionFactory
	{
		ITransaction Get(IConnectionDetails connection);

		Task<ITransaction> GetAsync(IConnectionDetails connection);
	}
}

