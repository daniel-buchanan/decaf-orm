using System;
using System.Collections.Generic;
using System.Linq;
using pdq.core.common.Connections;
using pdq.core.common.Logging;

namespace pdq.core.common
{
	public class Transient : ITransient
	{
        private readonly IConnection connection;
        private readonly ITransaction transaction;
        private readonly ILoggerProxy logger;
        private readonly IFluentApiCache fluentApiCache;
        private readonly List<IQuery> queries;

		public Transient(
            ITransaction transaction,
            ILoggerProxy logger,
            IFluentApiCache fluentApiCache)
		{
            this.connection = transaction.Connection;
            this.transaction = transaction;
            this.logger = logger;
            this.fluentApiCache = fluentApiCache;
            this.queries = new List<IQuery>();

            Id = Guid.NewGuid();

            this.logger.Debug($"Transient({Id}) :: Created");
		}

        public Guid Id { get; private set; }

        public void Dispose()
        {
            if(this.queries.Any(q => q.Status != QueryStatus.Executed))
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
                if(this.transaction.CloseTransactionOnCommitOrRollback)
                {
                    this.logger.Debug($"Transient({Id}) :: Closing Connection after Commit or Rollback");
                    this.connection.Close();
                }
            }

            this.logger.Debug($"Transient({Id}) :: Disposed");
        }

        public IQuery Query()
        {
            var query = common.Query.Create(logger, this);
            this.queries.Add(query);
            return query;
        }

        T ITransient.GetFluent<T>() => this.fluentApiCache.Get<T>();
    }
}

