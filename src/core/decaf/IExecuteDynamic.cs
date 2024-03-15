using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace decaf
{
	public interface IExecuteDynamic : IGetSql
	{
		IEnumerable<dynamic> AsEnumerable();

        Task<IEnumerable<dynamic>> AsEnumerableAsync(CancellationToken cancellationToken = default);

		IList<dynamic> ToList();

		Task<IList<dynamic>> ToListAsync(CancellationToken cancellationToken = default);

		dynamic FirstOrDefault();

		Task<dynamic> FirstOrDefaultAsync(CancellationToken cancellationToken = default);

		dynamic First();

		Task<dynamic> FirstAsync(CancellationToken cancellationToken = default);

		dynamic Single();

		Task<dynamic> SingleAsync(CancellationToken cancellationToken = default);

		dynamic SingleOrDefault();

		Task<dynamic> SingleOrDefaultAsync(CancellationToken cancellationToken = default);

		IExecute<T> Typed<T>();
	}
}

