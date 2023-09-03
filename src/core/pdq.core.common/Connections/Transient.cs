using System;
using System.Collections.Generic;
using System.Linq;
using pdq.common.Logging;
using pdq.common.Utilities;

namespace pdq.common.Connections
{
    public class Transient : ITransientInternal
	{
        private readonly IConnection connection;
        private readonly ITransactionInternal transaction;
        private readonly Action<Guid> notifyDisposed;
        private readonly ISqlFactory sqlFactory;
        private readonly ILoggerProxy logger;
        private readonly PdqOptions options;
        private readonly IHashProvider hashProvider;
        private readonly List<IQueryContainer> queries;

		private Transient(
            ITransaction transaction,
            ISqlFactory sqlFactory,
            ILoggerProxy logger,
            IHashProvider hashProvider,
            PdqOptions options,
            Action<Guid> notifyDisposed)
		{
            this.connection = transaction.Connection;
            this.transaction = transaction as ITransactionInternal;
            this.sqlFactory = sqlFactory;
            this.logger = logger;
            this.options = options;
            this.hashProvider = hashProvider;
            this.notifyDisposed = notifyDisposed;
            this.queries = new List<IQueryContainer>();

            Id = Guid.NewGuid();
            this.logger.Debug($"Transient({Id}) :: Created");
		}

        /// <inheritdoc />
        public Guid Id { get; private set; }

        /// <inheritdoc />
        IConnection ITransientInternal.Connection => this.connection;

        /// <inheritdoc />
        ITransaction ITransientInternal.Transaction => this.transaction;

        /// <inheritdoc />
        ISqlFactory ITransientInternal.SqlFactory => this.sqlFactory;

        public static ITransient Create(
            ITransaction transaction,
            ISqlFactory sqlFactory,
            ILoggerProxy logger,
            IHashProvider hashProvider,
            PdqOptions options,
            Action<Guid> notifyDisposed)
            => new Transient(transaction, sqlFactory, logger, hashProvider, options, notifyDisposed);

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (this.queries.Any(q => q.Status != QueryStatus.Executed))
            {
                this.logger.Warning($"Transient({Id}) :: One or more queries have not been executed.");
            }

            try
            {
                this.logger.Debug($"Transient({Id}) :: Committing Transaction");
                this.transaction.Commit();
            }
            catch (Exception commitException)
            {
                this.logger.Error(commitException, $"Transient({Id}) :: Committing Transaction Failed");
                try
                {
                    this.logger.Debug($"Transient({Id}) :: Rolling back Transaction");
                    this.transaction.Rollback();
                }
                catch (Exception rollbackException)
                {
                    this.logger.Error(rollbackException, $"Transient({Id}) :: Rolling back Transaction Failed");
                }
            }
            finally
            {
                if (this.transaction.CloseConnectionOnCommitOrRollback)
                {
                    this.logger.Debug($"Transient({Id}) :: Closing Connection after Commit or Rollback");
                    this.connection.Close();
                }
            }

            this.logger.Debug($"Transient({Id}) :: Disposed");
            this.notifyDisposed(this.Id);
        }

        /// <inheritdoc />
        public IQueryContainer Query() => Query(disposeTransientOnDispose: false);

        /// <inheritdoc />
        public IQueryContainer Query(bool disposeTransientOnDispose)
        {
            var query = QueryContainer.Create(this, this.logger, this.hashProvider, this.options, disposeTransientOnDispose);
            this.logger.Debug($"Transient({Id}) :: Creating new Query");
            this.queries.Add(query);
            return query;
        }

        public void NotifyQueryDisposed(Guid queryId)
        {
            var found = this.queries.FirstOrDefault(q => q.Id == queryId);
            if(found == null)
            {
                this.logger.Debug($"Transient({Id}) :: Cound not find query with Id - {queryId}");
                return;
            }

            this.queries.Remove(found);

            if (this.queries.Count != 0) return;

            this.Dispose();
        }
    }
}

