using System.Threading.Tasks;
using pdq.common.Connections;
using pdq.common.Exceptions;
using pdq.common.Logging;
using pdq.common.Utilities;

namespace pdq.common
{
    public class Pdq : IPdq
	{
        private readonly ILoggerProxy logger;
        private readonly ITransientFactory transientFactory;
        private readonly IConnectionDetails injectedConnectionDetails;

		public Pdq(
            ILoggerProxy logger,
            ITransientFactory transientFactory)
		{
            this.logger = logger;
            this.transientFactory = transientFactory;
		}

        public Pdq(
            ILoggerProxy logger,
            ITransientFactory transientFactory,
            IConnectionDetails connectionDetails)
            : this(logger, transientFactory)
        {
            this.injectedConnectionDetails = connectionDetails;
        }

        private ITransient GetTransient(IConnectionDetails connectionDetails = null)
            => GetTransientAsync(connectionDetails).WaitFor();

        private async Task<ITransient> GetTransientAsync(IConnectionDetails connectionDetails = null)
        {
            if (this.injectedConnectionDetails == null &&
               connectionDetails == null)
            {
                this.logger.Error("ConnectionDetails could not be found, either not injected or provided to method");
                throw new MissingConnectionDetailsException("ConnectionDetails were not injected or provided. Please ensure that either method is used.");
            }

            IConnectionDetails connectionDetailsToUse;
            if (connectionDetails != null) connectionDetailsToUse = connectionDetails;
            else connectionDetailsToUse = this.injectedConnectionDetails;

            return await this.transientFactory.CreateAsync(connectionDetailsToUse);
        }

        /// <inheritdoc/>
        public ITransient Begin()
            => GetTransient();

        /// <inheritdoc/>
        public ITransient Begin(IConnectionDetails connectionDetails)
            => GetTransient(connectionDetails);

        /// <inheritdoc/>
        public Task<ITransient> BeginAsync()
            => GetTransientAsync();

        /// <inheritdoc/>
        public Task<ITransient> BeginAsync(IConnectionDetails connectionDetails)
            => GetTransientAsync(connectionDetails);

        /// <inheritdoc/>
        public IQueryContainer BeginQuery()
            => GetTransient().Query();

        /// <inheritdoc/>
        public IQueryContainer BeginQuery(IConnectionDetails connectionDetails)
            => GetTransient(connectionDetails).Query();

        /// <inheritdoc/>
        public async Task<IQueryContainer> BeginQueryAsync()
        {
            var t = await GetTransientAsync();
            return t.Query();
        }

        /// <inheritdoc/>
        public async Task<IQueryContainer> BeginQueryAsync(IConnectionDetails connectionDetails)
        {
            var t = await GetTransientAsync(connectionDetails);
            return t.Query();
        }
    }
}

