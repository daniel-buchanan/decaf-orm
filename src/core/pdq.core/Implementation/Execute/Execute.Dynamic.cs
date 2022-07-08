using System.Collections.Generic;
using System.Threading.Tasks;
using pdq.common;
using Dapper;
using System.Linq;
using pdq.common.Utilities;

namespace pdq.Implementation
{
    internal class ExecuteDynamic : ExecuteBase, IExecuteDynamic
	{
		protected ExecuteDynamic(IQueryInternal query) : base(query) { }

        /// <inheritdoc/>
        public IEnumerable<dynamic> AsEnumerable() => AsEnumerableAsync().WaitFor();

        /// <inheritdoc/>
        public async Task<IEnumerable<dynamic>> AsEnumerableAsync()
            => await ExecuteAsync((s, p, t) => GetConnection().QueryAsync(s, p, t));

        /// <inheritdoc/>
        public dynamic First() => FirstAsync().WaitFor();

        /// <inheritdoc/>
        public async Task<dynamic> FirstAsync()
            => await ExecuteAsync((s, p, t) => GetConnection().QueryFirstAsync(s, p, t));

        /// <inheritdoc/>
        public dynamic FirstOrDefault() => FirstOrDefaultAsync().WaitFor();

        /// <inheritdoc/>
        public async Task<dynamic> FirstOrDefaultAsync()
            => await ExecuteAsync((s, p, t) => GetConnection().QueryFirstOrDefaultAsync(s, p, t));

        /// <inheritdoc/>
        public dynamic Single() => SingleAsync().WaitFor();

        /// <inheritdoc/>
        public async Task<dynamic> SingleAsync()
            => await ExecuteAsync((s, p, t) => GetConnection().QuerySingleAsync(s, p, t));

        /// <inheritdoc/>
        public dynamic SingleOrDefault() => SingleOrDefaultAsync().WaitFor();

        /// <inheritdoc/>
        public async Task<dynamic> SingleOrDefaultAsync()
            => await ExecuteAsync((s, p, t) => GetConnection().QuerySingleOrDefaultAsync(s, p, t));

        /// <inheritdoc/>
        public IList<dynamic> ToList() => ToListAsync().WaitFor();

        /// <inheritdoc/>
        public async Task<IList<dynamic>> ToListAsync()
        {
            var result = await AsEnumerableAsync();
            return result.ToList();
        }

        /// <inheritdoc/>
        public new string GetSql() => base.GetSql();

        public static IExecuteDynamic Create(IQueryInternal query) => new ExecuteDynamic(query);
    }
}

