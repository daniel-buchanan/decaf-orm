using System.Threading;
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
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IConnectionDetails injectedConnectionDetails;

		public Pdq(
            ILoggerProxy logger,
            IUnitOfWorkFactory unitOfWorkFactory)
		{
            this.logger = logger;
            this.unitOfWorkFactory = unitOfWorkFactory;
		}

        public Pdq(
            ILoggerProxy logger,
            IUnitOfWorkFactory unitOfWorkFactory,
            IConnectionDetails connectionDetails)
            : this(logger, unitOfWorkFactory)
        {
            this.injectedConnectionDetails = connectionDetails;
        }

        private IUnitOfWork GetUnitOfWork(IConnectionDetails connectionDetails = null)
            => GetUnitOfWorkAsync(connectionDetails).WaitFor();

        private async Task<IUnitOfWork> GetUnitOfWorkAsync(
            IConnectionDetails connectionDetails = null,
            CancellationToken cancellationToken = default)
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

            return await this.unitOfWorkFactory.CreateAsync(connectionDetailsToUse, cancellationToken);
        }

        /// <inheritdoc/>
        public IUnitOfWork Begin()
            => GetUnitOfWork();

        /// <inheritdoc/>
        public IUnitOfWork Begin(IConnectionDetails connectionDetails)
            => GetUnitOfWork(connectionDetails);

        /// <inheritdoc/>
        public Task<IUnitOfWork> BeginAsync(CancellationToken cancellationToken = default)
            => GetUnitOfWorkAsync(cancellationToken: cancellationToken);

        /// <inheritdoc/>
        public async Task<IUnitOfWork> BeginAsync(
            IConnectionDetails connectionDetails,
            CancellationToken cancellationToken = default)
            => await GetUnitOfWorkAsync(connectionDetails, cancellationToken);

        /// <inheritdoc/>
        public IQueryContainer BeginQuery()
            => GetUnitOfWork().Query();

        /// <inheritdoc/>
        public IQueryContainer BeginQuery(IConnectionDetails connectionDetails)
            => GetUnitOfWork(connectionDetails).Query();

        /// <inheritdoc/>
        public async Task<IQueryContainer> BeginQueryAsync(CancellationToken cancellationToken = default)
            => await BeginQueryAsync(null, cancellationToken);

        /// <inheritdoc/>
        public async Task<IQueryContainer> BeginQueryAsync(
            IConnectionDetails connectionDetails,
            CancellationToken cancellationToken = default)
        {
            var t = await GetUnitOfWorkAsync(connectionDetails, cancellationToken);
            return await t.QueryAsync(cancellationToken);
        }
    }
}

