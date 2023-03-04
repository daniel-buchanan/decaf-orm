using System;
using System.Collections.Generic;
using System.Linq;
using pdq.common.Connections;
using pdq.common.Logging;
using pdq.common.Utilities;

namespace pdq.common
{
	public class Transient : ITransientInternal
	{
        private readonly IConnection connection;
        private readonly ITransactionInternal transaction;
        private readonly ITransientFactoryInternal factory;
        private readonly ISqlFactory sqlFactory;
        private readonly ILoggerProxy logger;
        private readonly PdqOptions options;
        private readonly IHashProvider hashProvider;
        private readonly List<IQueryContainer> queries;

		private Transient(
            ITransientFactory factory,
            ITransaction transaction,
            ISqlFactory sqlFactory,
            ILoggerProxy logger,
            IHashProvider hashProvider,
            PdqOptions options)
		{
            this.factory = factory as ITransientFactoryInternal;
            this.connection = transaction.Connection;
            this.transaction = transaction as ITransactionInternal;
            this.sqlFactory = sqlFactory;
            this.logger = logger;
            this.options = options;
            this.hashProvider = hashProvider;
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
            ITransientFactory factory,
            ITransaction transaction,
            ISqlFactory sqlFactory,
            ILoggerProxy logger,
            IHashProvider hashProvider,
            PdqOptions options)
            => new Transient(factory, transaction, sqlFactory, logger, hashProvider, options);

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
            this.factory.NotifyTransientDisposed(this.Id);
        }

        /// <inheritdoc />
        public IQueryContainer Query()
        {
            var query = QueryFramework.Create(this.options, this.logger, this, this.hashProvider);
            this.logger.Debug($"Transient({Id}) :: Creating new Query");
            this.queries.Add(query);
            return query;
        }
    }
}

