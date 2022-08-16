using System;
using System.Data;
using System.Threading.Tasks;
using pdq.common;

namespace pdq.Implementation
{
    public abstract class ExecuteBase<TContext> :
        IGetSql
        where TContext: IQueryContext
	{
        internal IQueryInternal query;
        internal TContext context;
        protected ISqlFactory sqlFactory;

		protected ExecuteBase(
            IQuery query,
            TContext context,
            ISqlFactory sqlFactory)
		{
            this.query = query as IQueryInternal;
            this.context = context;
            this.sqlFactory = sqlFactory;
		}

        protected IDbTransaction GetTransaction()
        {
            var internalTransient = this.query.Transient as ITransientInternal;
            return internalTransient.Transaction.GetUnderlyingTransaction();
        }

        protected IDbConnection GetConnection()
        {
            var internalTransient = this.query.Transient as ITransientInternal;
            return internalTransient.Connection.GetUnderlyingConnection();
        }

        /// <summary>
        /// Execute a function on a DBTransaction
        /// </summary>
        /// <typeparam name="T">The return type for the function.</typeparam>
        /// <param name="func">The function to execute.</param>
        /// <returns>The result of the function</returns>
        protected async Task<T> ExecuteAsync<T>(
            Func<string, object, IDbConnection, IDbTransaction, Task<T>> func)
        {
            var template = this.sqlFactory.ParseContext(this.context);
            var connection = GetConnection();
            var transaction = GetTransaction();

            return await func(template.Sql, template.Parameters, connection, transaction);
        }

        /// <inheritdoc/>
        public string GetSql()
        {
            var template = this.sqlFactory.ParseContext(this.context);
            return template?.Sql;
        }
    }
}

