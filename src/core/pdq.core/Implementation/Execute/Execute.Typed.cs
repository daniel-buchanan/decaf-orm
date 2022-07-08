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

        /// <inheritdoc/>
        public IEnumerable<T> AsEnumerable() => AsEnumerableAsync<T>().WaitFor();

        /// <inheritdoc/>
        public Task<IEnumerable<T>> AsEnumerableAsync() => AsEnumerableAsync<T>();

        /// <inheritdoc/>
        public T First() => FirstAsync().WaitFor();

        /// <inheritdoc/>
        public Task<T> FirstAsync() => FirstAsync<T>();

        /// <inheritdoc/>
        public T FirstOrDefault() => FirstOrDefaultAsync().WaitFor();

        /// <inheritdoc/>
        public Task<T> FirstOrDefaultAsync() => FirstOrDefaultAsync<T>();

        /// <inheritdoc/>
        public T Single() => SingleAsync().WaitFor();

        /// <inheritdoc/>
        public Task<T> SingleAsync() => SingleAsync<T>();

        /// <inheritdoc/>
        public T SingleOrDefault() => SingleOrDefaultAsync().WaitFor();

        /// <inheritdoc/>
        public Task<T> SingleOrDefaultAsync() => SingleOrDefaultAsync<T>();

        /// <inheritdoc/>
        public IList<T> ToList() => ToListAsync().WaitFor();

        /// <inheritdoc/>
        public Task<IList<T>> ToListAsync() => ToListAsync<T>();

        /// <inheritdoc/>
        void IExecute<T>.Execute() => ExecuteAsync().WaitFor();
    }
}

