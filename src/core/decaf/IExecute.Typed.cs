using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace decaf;

public interface IExecute<T> : IGetSql
{
	IEnumerable<T> AsEnumerable();

	Task<IEnumerable<T>> AsEnumerableAsync(CancellationToken cancellationToken = default);

	IList<T> ToList();

	Task<IList<T>> ToListAsync(CancellationToken cancellationToken = default);

	T FirstOrDefault();

	Task<T> FirstOrDefaultAsync(CancellationToken cancellationToken = default);

	T First();

	Task<T> FirstAsync(CancellationToken cancellationToken = default);

	T Single();

	Task<T> SingleAsync(CancellationToken cancellationToken = default);

	T SingleOrDefault();

	Task<T> SingleOrDefaultAsync(CancellationToken cancellationToken = default);

	void Execute();

	Task ExecuteAsync(CancellationToken cancellationToken = default);

	IExecuteDynamic Dynamic();
}