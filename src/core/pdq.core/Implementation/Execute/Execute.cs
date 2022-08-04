using System.Collections.Generic;
using System.Threading.Tasks;
using pdq.common;
using Dapper;
using System.Linq;
using pdq.common.Utilities;

namespace pdq.Implementation
{
    internal class Execute<TContext> : ExecuteBase<TContext>, IExecute
        where TContext: IQueryContext
	{
		protected Execute(
            IQueryInternal query,
            TContext context)
            : base(query, context, query.SqlFactory) { }

        /// <inheritdoc/>
        public IExecuteDynamic Dynamic() => this;

        /// <inheritdoc/>
        public IEnumerable<T> AsEnumerable<T>()
            => AsEnumerableAsync<T>().WaitFor();

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> AsEnumerableAsync<T>()
            => await ExecuteAsync((s, p, c, t) => c.QueryAsync<T>(s, p, t));

        /// <inheritdoc/>
        public IEnumerable<dynamic> AsEnumerable()
            => AsEnumerableAsync().WaitFor();

        /// <inheritdoc/>
        public async Task<IEnumerable<dynamic>> AsEnumerableAsync()
            => await ExecuteAsync((s, p, c, t) => c.QueryAsync(s, p, t));

        /// <inheritdoc/>
        public T First<T>()
            => FirstAsync<T>().WaitFor();

        /// <inheritdoc/>
        public async Task<T> FirstAsync<T>()
            => await ExecuteAsync((s, p, c, t) => c.QueryFirstAsync<T>(s, p, t));

        /// <inheritdoc/>
        public dynamic First()
            => FirstAsync().WaitFor();

        /// <inheritdoc/>
        public async Task<dynamic> FirstAsync()
            => await ExecuteAsync((s, p, c, t) => c.QueryFirstAsync(s, p, t));

        /// <inheritdoc/>
        public T FirstOrDefault<T>()
            => FirstOrDefaultAsync<T>().WaitFor();

        /// <inheritdoc/>
        public async Task<T> FirstOrDefaultAsync<T>()
            => await ExecuteAsync((s, p, c, t) => c.QueryFirstOrDefaultAsync<T>(s, p, t));

        /// <inheritdoc/>
        public dynamic FirstOrDefault()
            => FirstOrDefaultAsync().WaitFor();

        /// <inheritdoc/>
        public async Task<dynamic> FirstOrDefaultAsync()
            => await ExecuteAsync((s, p, c, t) => c.QueryFirstOrDefaultAsync(s, p, t));

        /// <inheritdoc/>
        public T Single<T>()
            => SingleAsync<T>().WaitFor();

        /// <inheritdoc/>
        public async Task<T> SingleAsync<T>()
            => await ExecuteAsync((s, p, c, t) => c.QuerySingleAsync<T>(s, p, t));

        /// <inheritdoc/>
        public dynamic Single()
            => SingleAsync().WaitFor();

        /// <inheritdoc/>
        public async Task<dynamic> SingleAsync()
            => await ExecuteAsync((s, p, c, t) => c.QuerySingleAsync(s, p, t));

        /// <inheritdoc/>
        public T SingleOrDefault<T>()
            => SingleOrDefaultAsync<T>().WaitFor();

        /// <inheritdoc/>
        public async Task<T> SingleOrDefaultAsync<T>()
            => await ExecuteAsync((s, p, c, t) => c.QuerySingleOrDefaultAsync<T>(s, p, t));

        /// <inheritdoc/>
        public dynamic SingleOrDefault()
            => SingleOrDefaultAsync().WaitFor();

        /// <inheritdoc/>
        public async Task<dynamic> SingleOrDefaultAsync()
            => await ExecuteAsync((s, p, c, t) => c.QuerySingleOrDefaultAsync(s, p, t));

        /// <inheritdoc/>
        public IList<T> ToList<T>()
            => ToListAsync<T>().WaitFor();

        /// <inheritdoc/>
        public async Task<IList<T>> ToListAsync<T>()
            => (await AsEnumerableAsync<T>()).ToList();

        /// <inheritdoc/>
        public IList<dynamic> ToList()
            => ToListAsync().WaitFor();

        /// <inheritdoc/>
        public async Task<IList<dynamic>> ToListAsync()
            => (await AsEnumerableAsync()).ToList();

        /// <inheritdoc/>
        public IExecute<T> Typed<T>()
            => Execute<T, TContext>.Create(this.query, this.context);

        /// <inheritdoc/>
        void IExecute.Execute()
            => ExecuteAsync().WaitFor();

        /// <inheritdoc/>
        public async Task ExecuteAsync()
            => await ExecuteAsync((s, p, c, t) => c.ExecuteAsync(s, p, t));

        /// <inheritdoc/>
        public new string GetSql()
            => base.GetSql();

    }
}

