using System;
using System.Data;
using System.Threading.Tasks;
using pdq.common;

namespace pdq.Implementation
{
    public abstract class ExecuteBase
	{
        protected IQueryInternal query;

		protected ExecuteBase(IQueryInternal query)
		{
            this.query = query;
		}

        protected IDbConnection GetConnection() => this.query.Transient.Connection.GetUnderlyingConnection();

        protected async Task<T> ExecuteAsync<T>(Func<string, object, IDbTransaction, Task<T>> func)
        {
            var sql = GetSql();
            var parameters = GetSqlParameters();
            var transaction = this.query.Transient.Transaction.GetUnderlyingTransaction();

            return await func(sql, parameters, transaction);
        }

        protected string GetSql()
        {
            
            throw new NotImplementedException();
        }

        private object GetSqlParameters()
        {
            throw new NotImplementedException();
        }
    }
}

