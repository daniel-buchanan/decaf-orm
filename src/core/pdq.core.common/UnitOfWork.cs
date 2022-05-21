using pdq.core.common.Logging;

namespace pdq.core.common
{
	public class UnitOfWork : IUnitOfWork
	{
        private readonly ILoggerProxy logger;
        private readonly ITransientFactory transientFactory;
        private readonly IConnectionDetails connectionDetails;

		public UnitOfWork(
            ILoggerProxy logger,
            ITransientFactory transientFactory,
            IConnectionDetails connectionDetails)
		{
            this.logger = logger;
            this.transientFactory = transientFactory;
            this.connectionDetails = connectionDetails;
		}

        public ITransient Begin()
        {
            this.logger.Debug("Creating Transient");
            return this.transientFactory.Create(this.connectionDetails);
        }
    }
}

