using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using decaf.common.Logging;
using decaf.common.Utilities;

namespace decaf.common.Connections
{
    public class UnitOfWork : IUnitOfWorkExtended
    {
        private readonly IConnection connection;
        private readonly ITransactionInternal transaction;
        private readonly Action<Guid> notifyDisposed;
        private readonly ISqlFactory sqlFactory;
        private readonly ILoggerProxy logger;
        private readonly DecafOptions options;
        private readonly IHashProvider hashProvider;
        private readonly List<IQueryContainer> queries;

        private UnitOfWork(
            ITransaction transaction,
            ISqlFactory sqlFactory,
            ILoggerProxy logger,
            IHashProvider hashProvider,
            DecafOptions options,
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
            this.logger.Debug($"UnitOfWork({Id}) :: Created");
        }

        /// <inheritdoc />
        public Guid Id { get; private set; }

        /// <inheritdoc />
        IConnection IUnitOfWorkExtended.Connection => this.connection;

        /// <inheritdoc />
        ITransaction IUnitOfWorkExtended.Transaction => this.transaction;

        /// <inheritdoc />
        ISqlFactory IUnitOfWorkExtended.SqlFactory => this.sqlFactory;

        public static IUnitOfWork Create(
            ITransaction transaction,
            ISqlFactory sqlFactory,
            ILoggerProxy logger,
            IHashProvider hashProvider,
            DecafOptions options,
            Action<Guid> notifyDisposed)
            => new UnitOfWork(transaction, sqlFactory, logger, hashProvider, options, notifyDisposed);

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
                this.logger.Warning($"UnitOfWork({Id}) :: One or more queries have not been executed.");
            }

            try
            {
                this.logger.Debug($"UnitOfWork({Id}) :: Committing Transaction");
                this.transaction.Commit();
            }
            catch (Exception commitException)
            {
                this.logger.Error(commitException, $"UnitOfWork({Id}) :: Committing Transaction Failed");
                try
                {
                    this.logger.Debug($"UnitOfWork({Id}) :: Rolling back Transaction");
                    this.transaction.Rollback();
                }
                catch (Exception rollbackException)
                {
                    this.logger.Error(rollbackException, $"UnitOfWork({Id}) :: Rolling back Transaction Failed");
                }
            }
            finally
            {
                if (this.transaction.CloseConnectionOnCommitOrRollback)
                {
                    this.logger.Debug($"UnitOfWork({Id}) :: Closing Connection after Commit or Rollback");
                    this.connection.Close();
                }
            }

            this.logger.Debug($"UnitOfWork({Id}) :: Disposed");
            this.notifyDisposed(this.Id);
        }

        /// <inheritdoc />
        public IQueryContainer Query()
            => QueryAsync().WaitFor();

        public Task<IQueryContainer> QueryAsync(CancellationToken cancellationToken = default)
        {
            var query = QueryContainer.Create(this, this.logger, this.hashProvider, this.options);
            this.logger.Debug($"UnitOfWork({Id}) :: Creating new Query");
            this.queries.Add(query);
            return Task.FromResult(query);
        }

        /// <inheritdoc />
        public void NotifyQueryDisposed(Guid queryId)
        {
            var found = this.queries.FirstOrDefault(q => q.Id == queryId);
            if (found == null)
            {
                this.logger.Debug($"UnitOfWork({Id}) :: Cound not find query with Id - {queryId}");
                return;
            }

            this.queries.Remove(found);

            if (this.queries.Count != 0) return;

            this.Dispose();
        }
    }
}

