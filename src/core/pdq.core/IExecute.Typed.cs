using System.Collections.Generic;
using System.Threading.Tasks;

namespace pdq
{
	public interface IExecute<T> : IGetSql
	{
		IEnumerable<T> AsEnumerable();

		Task<IEnumerable<T>> AsEnumerableAsync();

		IList<T> ToList();

		Task<IList<T>> ToListAsync();

		T FirstOrDefault();

		Task<T> FirstOrDefaultAsync();

		T First();

		Task<T> FirstAsync();

		T Single();

		Task<T> SingleAsync();

		T SingleOrDefault();

		Task<T> SingleOrDefaultAsync();

		void Execute();

		Task ExecuteAsync();
	}
}

