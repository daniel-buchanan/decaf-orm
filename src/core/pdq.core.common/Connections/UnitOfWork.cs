using System.Threading.Tasks;
using pdq.common.Logging;

namespace pdq.common.Connections
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

        /// <inheritdoc />
        public ITransient Begin()
        {
            this.logger.Debug("Creating Transient");
            return this.transientFactory.Create(this.connectionDetails);
        }

        /// <inheritdoc />
        public async Task<ITransient> BeginAsync()
        {
            this.logger.Debug("Creating Transient (async)");
            return await this.transientFactory.CreateAsync(this.connectionDetails);
        }
    }
}

