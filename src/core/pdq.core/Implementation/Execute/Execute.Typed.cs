using System.Collections.Generic;
using System.Threading.Tasks;
using pdq.common;
using pdq.common.Utilities;

namespace pdq.Implementation
{
    internal class Execute<T> : Execute, IExecute<T>
	{
		private Execute(IQueryInternal query) : base(query) { }

        public static IExecute<T> Create(IQueryInternal query) => new Execute<T>(query);

        public IEnumerable<T> AsEnumerable() => AsEnumerableAsync<T>().WaitFor();

        public Task<IEnumerable<T>> AsEnumerableAsync() => AsEnumerableAsync<T>();

        public T First() => FirstAsync().WaitFor();

        public Task<T> FirstAsync() => FirstAsync<T>();

        public T FirstOrDefault() => FirstOrDefaultAsync().WaitFor();

        public Task<T> FirstOrDefaultAsync() => FirstOrDefaultAsync<T>();

        public T Single() => SingleAsync().WaitFor();

        public Task<T> SingleAsync() => SingleAsync<T>();

        public T SingleOrDefault() => SingleOrDefaultAsync().WaitFor();

        public Task<T> SingleOrDefaultAsync() => SingleOrDefaultAsync<T>();

        public IList<T> ToList() => ToListAsync().WaitFor();

        public Task<IList<T>> ToListAsync() => ToListAsync<T>();

        void IExecute<T>.Execute() => ExecuteAsync().WaitFor();
    }
}

