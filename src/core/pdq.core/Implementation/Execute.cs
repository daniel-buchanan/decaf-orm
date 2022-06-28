using System.Collections.Generic;
using System.Threading.Tasks;
using pdq.common;
using pdq.Implementation.Helpers;
using Dapper;
using System.Linq;

namespace pdq.Implementation
{
    internal class Execute : ExecuteBase, IExecute
	{
		protected Execute(IQueryInternal query) : base(query) { }

        public IExecuteDynamic AsDynamic() => ExecuteDynamic.Create(this.query);

        public IEnumerable<T> AsEnumerable<T>() => AsEnumerableAsync<T>().WaitFor();

        public async Task<IEnumerable<T>> AsEnumerableAsync<T>()
            => await ExecuteAsync((s, p, t) => GetConnection().QueryAsync<T>(s, p, t));

        public T First<T>() => FirstAsync<T>().WaitFor();

        public async Task<T> FirstAsync<T>()
            => await ExecuteAsync((s, p, t) => GetConnection().QueryFirstAsync<T>(s, p, t));

        public T FirstOrDefault<T>() => FirstOrDefaultAsync<T>().WaitFor();

        public async Task<T> FirstOrDefaultAsync<T>()
            => await ExecuteAsync((s, p, t) => GetConnection().QueryFirstOrDefaultAsync<T>(s, p, t));

        public T Single<T>() => SingleAsync<T>().WaitFor();

        public async Task<T> SingleAsync<T>()
            => await ExecuteAsync((s, p, t) => GetConnection().QuerySingleAsync<T>(s, p, t));

        public T SingleOrDefault<T>() => SingleOrDefaultAsync<T>().WaitFor();

        public async Task<T> SingleOrDefaultAsync<T>()
            => await ExecuteAsync((s, p, t) => GetConnection().QuerySingleOrDefaultAsync<T>(s, p, t));

        public IList<T> ToList<T>() => ToListAsync<T>().WaitFor();

        public async Task<IList<T>> ToListAsync<T>()
        {
            var enumerable = await AsEnumerableAsync<T>();
            return enumerable.ToList();
        }

        void IExecute.Execute() => ExecuteAsync().WaitFor();

        public async Task ExecuteAsync()
            => await ExecuteAsync((s, p, t) => GetConnection().ExecuteAsync(s, p, t));

        public new string GetSql() => base.GetSql();

        public static IExecute Create(IQueryInternal query) => new Execute(query);
    }
}

