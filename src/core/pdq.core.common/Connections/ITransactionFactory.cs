using System;
using System.Threading.Tasks;

namespace pdq.common.Connections
{
	public interface ITransactionFactory
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionDetails"></param>
        /// <returns></returns>
        ITransaction Get(IConnectionDetails connectionDetails);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionDetails"></param>
        /// <returns></returns>
        Task<ITransaction> GetAsync(IConnectionDetails connectionDetails);
	}
}

