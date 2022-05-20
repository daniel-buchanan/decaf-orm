using System;
using System.Threading.Tasks;

namespace pdq.core.common
{
	public interface ITransientFactory
	{
		ITransient Create(IConnectionDetails connectionDetails);

		Task<ITransient> CreateAsync(IConnectionDetails connectionDetails);
	}
}

