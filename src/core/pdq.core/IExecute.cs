using System.Collections.Generic;
using System.Threading.Tasks;

namespace pdq
{
	public interface IExecute : IBuilder
	{
		IEnumerable<T> AsEnumerable<T>();

		Task<IEnumerable<T>> AsEnumerableAsync<T>();

		IList<T> ToList<T>();

		Task<IList<T>> ToListAsync<T>();

		T FirstOrDefault<T>();

		Task<T> FirstOrDefaultAsync<T>();

		T First<T>();

		Task<T> FirstAsync<T>();

		T Single<T>();

		Task<T> SingleAsync<T>();

		T SingleOrDefault<T>();

		Task<T> SingleOrDefaultAsync<T>();

		void Execute();

		Task ExecuteAsync();

		IExecuteDynamic AsDynamic();
	}
}

