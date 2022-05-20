using System;
using pdq.core.common.Connections;
using pdq.core.Logging;

namespace pdq.core.common
{
	public class Transient : ITransient
	{
        private readonly IConnection connection;
        private readonly ITransaction transaction;
        private readonly ILoggerProxy logger;

		public Transient(
            ITransaction transaction,
            ILoggerProxy logger)
		{
            this.connection = transaction.Connection;
            this.transaction = transaction;
            this.logger = logger;
		}

        public void Dispose()
        {
            try
            {
                this.logger.Debug("Committing Transaction");
                this.transaction.Commit();
            }
            catch (Exception commitException)
            {
                this.logger.Error(commitException, "Committing Transaction Failed");
                try
                {
                    this.logger.Debug("Rolling back Transaction");
                    this.transaction.Rollback();
                }
                catch (Exception rollbackException)
                {
                    this.logger.Error(rollbackException, "Rolling back Transaction Failed");
                }
            }
            finally
            {
                if(this.transaction.CloseTransactionOnCommitOrRollback)
                {
                    this.logger.Debug("Closing Connection after Commit or Rollback");
                    this.connection.Close();
                }
            }
        }

        public IQuery Query()
        {
            throw new NotImplementedException();
        }
    }
}

