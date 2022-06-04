using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pdq.common.Connections;
using pdq.common.Logging;

namespace pdq.common
{
	public sealed class TransientFactory : ITransientFactory
	{
        private readonly List<ITransient> tracker;
        private readonly ILoggerProxy logger;
        private readonly ITransactionFactory transactionFactory;

		public TransientFactory(
            ILoggerProxy logger,
            ITransactionFactory transactionFactory)
		{
            this.logger = logger;
            this.transactionFactory = transactionFactory;
		}

        public ITransient Create(IConnectionDetails connectionDetails)
        {
            var t = this.CreateAsync(connectionDetails);
            t.Wait();
            return t.Result;
        }

        public async Task<ITransient> CreateAsync(IConnectionDetails connectionDetails)
        {
            this.logger.Debug("TransientFactory :: Getting Transaction");
            var transaction = await this.transactionFactory.GetAsync(connectionDetails);
            var transient = Transient.Create(this, transaction, this.logger);
            this.logger.Debug($"TransientFactory :: Transient ({transient.Id}) Tracked");
            this.tracker.Add(transient);
            return transient;
        }

        public void Dispose()
        {
            this.logger.Debug($"TransientFactory :: Disposing Resources, {this.tracker.Count} Transients");
            tracker.Clear();
        }

        void ITransientFactory.NotifyTransientDisposed(Guid id)
        {
            var transient = this.tracker.FirstOrDefault(t => t.Id == id);
            if (transient == null)
            {
                this.logger.Debug($"TransientFactory :: Notified Transient ({id}) disposed, but was not tracked");
                return;
            }

            this.logger.Debug($"TransientFactory :: Notified Transient({id}) disposed, tracking removed");
            this.tracker.Remove(transient);
        }
    }
}