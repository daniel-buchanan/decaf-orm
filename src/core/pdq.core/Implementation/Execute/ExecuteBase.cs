using System;
using System.Data;
using System.Threading.Tasks;
using pdq.common;

namespace pdq.Implementation
{
    public abstract class ExecuteBase
	{
        internal IQueryInternal query;

		protected ExecuteBase(IQuery query)
		{
            this.query = query as IQueryInternal;
		}

        protected IDbConnection GetConnection()
        {
            var internalTransient = this.query.Transient as ITransientInternal;
            return internalTransient.Connection.GetUnderlyingConnection();
        }

        protected IDbTransaction GetTransatction()
        {
            var internalTransient = this.query.Transient as ITransientInternal;
            return internalTransient.Transaction.GetUnderlyingTransaction();
        }

        /// <summary>
        /// Execute a function on a DBTransaction
        /// </summary>
        /// <typeparam name="T">The return type for the function.</typeparam>
        /// <param name="func">The function to execute.</param>
        /// <returns>The result of the function</returns>
        protected async Task<T> ExecuteAsync<T>(Func<string, object, IDbTransaction, Task<T>> func)
        {
            var sql = GetSql();
            var parameters = GetSqlParameters();
            var transaction = GetTransatction();

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

