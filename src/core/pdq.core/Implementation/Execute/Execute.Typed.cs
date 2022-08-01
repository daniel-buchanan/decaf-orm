using System.Collections.Generic;
using System.Threading.Tasks;
using pdq.common;
using pdq.common.Utilities;

namespace pdq.Implementation
{
    internal class Execute<TResult, TContext> : Execute<TContext>, IExecute<TResult>
        where TContext: IQueryContext
	{
		private Execute(
            IQueryInternal query,
            TContext context)
            : base(query, context, query.SqlFactory) { }

        public static IExecute<TResult> Create(
            IQueryInternal query,
            TContext context)
            => new Execute<TResult, TContext>(query, context);

        /// <inheritdoc/>
        public new IEnumerable<TResult> AsEnumerable() => AsEnumerableAsync<TResult>().WaitFor();

        /// <inheritdoc/>
        public new Task<IEnumerable<TResult>> AsEnumerableAsync() => AsEnumerableAsync<TResult>();

        /// <inheritdoc/>
        public new TResult First() => FirstAsync().WaitFor();

        /// <inheritdoc/>
        public new Task<TResult> FirstAsync() => FirstAsync<TResult>();

        /// <inheritdoc/>
        public new TResult FirstOrDefault() => FirstOrDefaultAsync().WaitFor();

        /// <inheritdoc/>
        public new Task<TResult> FirstOrDefaultAsync() => FirstOrDefaultAsync<TResult>();

        /// <inheritdoc/>
        public new TResult Single() => SingleAsync().WaitFor();

        /// <inheritdoc/>
        public new Task<TResult> SingleAsync() => SingleAsync<TResult>();

        /// <inheritdoc/>
        public new TResult SingleOrDefault() => SingleOrDefaultAsync().WaitFor();

        /// <inheritdoc/>
        public new Task<TResult> SingleOrDefaultAsync() => SingleOrDefaultAsync<TResult>();

        /// <inheritdoc/>
        public new IList<TResult> ToList() => ToListAsync().WaitFor();

        /// <inheritdoc/>
        public new Task<IList<TResult>> ToListAsync() => ToListAsync<TResult>();

        /// <inheritdoc/>
        void IExecute<TResult>.Execute() => ExecuteAsync().WaitFor();
    }
}

