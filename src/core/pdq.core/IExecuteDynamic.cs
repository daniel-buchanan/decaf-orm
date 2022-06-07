using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pdq
{
	public interface IExecuteDynamic
	{
		IEnumerable<dynamic> AsEnumerable();

		Task<IEnumerable<dynamic>> AsEnumerableAsync();

		IList<dynamic> ToList();

		Task<IList<dynamic>> ToListAsync();

		dynamic FirstOrDefault();

		Task<dynamic> FirstOrDefaultAsync();

		dynamic First();

		Task<dynamic> FirstAsync();

		dynamic Single();

		Task<dynamic> SingleAsync();

		dynamic SingleOrDefault();

		Task<dynamic> SingleOrDefaultAsync();
	}
}

