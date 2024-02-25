using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using decaf.common;
using decaf.common.Connections;
using decaf.common.Utilities;

namespace decaf.Implementation
{
    public abstract class ExecuteBase<TContext> :
        IGetSql
        where TContext: IQueryContext
	{
        internal IQueryContainerInternal query;
        internal TContext context;
        protected ISqlFactory sqlFactory;

		protected ExecuteBase(
            IQueryContainer query,
            TContext context,
            ISqlFactory sqlFactory)
		{
            this.query = query as IQueryContainerInternal;
            this.context = context;
            this.sqlFactory = sqlFactory;
		}

        protected IDbTransaction GetTransaction()
        {
            var internalTransient = this.query.UnitOfWork as IUnitOfWorkExtended;
            return internalTransient.Transaction.GetUnderlyingTransaction();
        }

        protected IDbConnection GetConnection()
        {
            var internalTransient = this.query.UnitOfWork as IUnitOfWorkExtended;
            return internalTransient.Connection.GetUnderlyingConnection();
        }

        /// <summary>
        /// Execute a function on a DBTransaction
        /// </summary>
        /// <typeparam name="T">The return type for the function.</typeparam>
        /// <param name="func">The function to execute.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result of the function</returns>
        protected async Task<T> ExecuteAsync<T>(
            Func<string, object, IDbConnection, IDbTransaction, CancellationToken, Task<T>> func,
            CancellationToken cancellationToken = default)
        {
            var template = this.sqlFactory.ParseTemplate(this.context);
            var parameters = this.sqlFactory.ParseParameters(this.context, template);
            var connection = GetConnection();
            var transaction = GetTransaction();

            return await func(template.Sql, parameters, connection, transaction, cancellationToken);
        }

        /// <inheritdoc/>
        public string GetSql()
        {
            var template = this.sqlFactory.ParseTemplate(this.context);
            return template.Sql;
        }

        /// <inheritdoc/>
        public Dictionary<string, object> GetSqlParameters()
        {
            var template = this.sqlFactory.ParseTemplate(this.context);
            var parameters = this.sqlFactory.ParseParameters(context, template) as DynamicDictionary;
            return parameters.ToDictionary();
        }
    }
}

