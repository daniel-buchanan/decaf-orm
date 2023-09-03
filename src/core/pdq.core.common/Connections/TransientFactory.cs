using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pdq.common.Logging;
using pdq.common.Utilities;

namespace pdq.common.Connections
{
    public sealed class TransientFactory : ITransientFactoryInternal
	{
        private readonly List<ITransient> tracker;
        private readonly PdqOptions options;
        private readonly ILoggerProxy logger;
        private readonly ITransactionFactory transactionFactory;
        private readonly ISqlFactory sqlFactory;
        private readonly IHashProvider hashProvider;

        public TransientFactory(
            PdqOptions options,
            ILoggerProxy logger,
            ITransactionFactory transactionFactory,
            ISqlFactory sqlFactory,
            IHashProvider hashProvider)
		{
            this.tracker = new List<ITransient>();
            this.options = options;
            this.logger = logger;
            this.transactionFactory = transactionFactory;
            this.sqlFactory = sqlFactory;
            this.hashProvider = hashProvider;
        }

        /// <inheritdoc/>
        public ITransient Create(IConnectionDetails connectionDetails) => CreateAsync(connectionDetails).WaitFor();

        /// <inheritdoc/>
        public async Task<ITransient> CreateAsync(IConnectionDetails connectionDetails)
        {
            this.logger.Debug("TransientFactory :: Getting Transaction");
            var transaction = await this.transactionFactory.GetAsync(connectionDetails);
            var transient = Transient.Create(
                transaction,
                this.sqlFactory,
                this.logger,
                this.hashProvider,
                this.options,
                NotifyTransientDisposed);
            this.logger.Debug($"TransientFactory :: Transient ({transient.Id}) Tracked");

            if(this.options.TrackTransients) this.tracker.Add(transient);

            return transient;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (!this.options.TrackTransients) return;

            this.logger.Debug($"TransientFactory :: Disposing Resources, {this.tracker.Count} Transients");
            tracker.Clear();
        }

        private void NotifyTransientDisposed(Guid id)
        {
            if (!this.options.TrackTransients) return;

            var transient = this.tracker.FirstOrDefault(t => t.Id == id);
            if (transient == null)
            {
                this.logger.Debug($"TransientFactory :: Notified Transient ({id}) disposed, but was not tracked");
                return;
            }

            this.logger.Debug($"TransientFactory :: Notified Transient({id}) disposed, tracking removed");
            this.tracker.Remove(transient);
        }

        /// <inheritdoc/>
        void ITransientFactoryInternal.NotifyTransientDisposed(Guid id)
            => this.NotifyTransientDisposed(id);
    }
}