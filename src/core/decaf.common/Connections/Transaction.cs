using System;
using System.Data;
using decaf.common.Logging;

namespace decaf.common.Connections
{
	public abstract class Transaction : ITransactionInternal
	{
        private readonly ILoggerProxy logger;
        private readonly DecafOptions options;
        protected readonly IConnection connection;
        private IDbTransaction transaction;

        protected Transaction(
            Guid id,
            ILoggerProxy logger,
            IConnection connection,
            DecafOptions options)
		{
            Id = id;
            this.logger = logger;
            this.connection = connection;
            this.options = options;
            this.State = TransactionState.Created;

            this.logger.Debug($"ITransaction({Id}) :: Transaction created");
		}

        /// <inheritdoc/>
        public IConnection Connection => this.connection;

        /// <inheritdoc/>
        public Guid Id { get; }

        /// <inheritdoc/>
        bool ITransactionInternal.CloseConnectionOnCommitOrRollback => this.options.CloseConnectionOnCommitOrRollback;

        /// <inheritdoc/>
        public void Begin()
        {
            if (this.transaction != null) return;

            this.logger.Debug($"ITransaction({Id}) :: Beginning Transaction");
            if(!options.LazyInitialiseConnections)
                this.transaction = GetUnderlyingTransaction();
            this.State = TransactionState.Begun;
        }

        /// <inheritdoc/>
        public void Commit()
        {
            try
            {
                if (options.LazyInitialiseConnections)
                    Begin();
                
                this.logger.Debug($"ITransaction({Id}) :: Committing Transaction");
                this.transaction.Commit();
                this.State = TransactionState.Committed;
                this.logger.Debug($"ITransaction({Id}) :: Commit SUCCEEDED");
            }
            catch (Exception commitEx)
            {
                try
                {
                    this.logger.Error(commitEx, $"ITransaction({Id}) :: Commit FAILED, attempting Rollback");
                    Rollback();
                }
                catch (Exception rollbackEx)
                {
                    this.logger.Error(rollbackEx, $"ITransaction({Id}) :: Rollback FAILED");
                }

                this.State = TransactionState.RolledBack;
            }
            finally
            {
                this.logger.Debug($"ITransaction({Id}) :: Finished Commit Process");
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            this.transaction = null;
            this.State = TransactionState.Disposed;
        }

        /// <inheritdoc/>
        public void Rollback()
        {
            try
            {
                this.logger.Debug($"ITransaction({Id}) :: Rolling back Transaction");
                this.transaction.Rollback();
                this.logger.Debug($"ITransaction({Id}) :: Rollback SUCCEEDED");
            }
            catch (Exception rollbackEx)
            {
                this.logger.Error(rollbackEx, $"ITransaction({Id}) :: Rollback FAILED");
            }
            finally
            {
                this.State = TransactionState.RolledBack;
            }
        }

        /// <inheritdoc/>
        public abstract IDbTransaction GetUnderlyingTransaction();

        /// <inheritdoc/>
        public TransactionState State { get; private set; }
    }
}

