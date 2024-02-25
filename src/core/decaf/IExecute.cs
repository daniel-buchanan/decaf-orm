using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace decaf
{
	public interface IExecute : IExecuteDynamic
	{
		IEnumerable<T> AsEnumerable<T>();

		Task<IEnumerable<T>> AsEnumerableAsync<T>(CancellationToken cancellationToken = default);

		IList<T> ToList<T>();

		Task<IList<T>> ToListAsync<T>(CancellationToken cancellationToken = default);

		T FirstOrDefault<T>();

		Task<T> FirstOrDefaultAsync<T>(CancellationToken cancellationToken = default);

		T First<T>();

		Task<T> FirstAsync<T>(CancellationToken cancellationToken = default);

		T Single<T>();

		Task<T> SingleAsync<T>(CancellationToken cancellationToken = default);

		T SingleOrDefault<T>();

		Task<T> SingleOrDefaultAsync<T>(CancellationToken cancellationToken = default);

		void Execute();

		Task ExecuteAsync(CancellationToken cancellationToken = default);

		IExecuteDynamic Dynamic();
	}
}

