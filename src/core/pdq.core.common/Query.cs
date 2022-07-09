using System;
using pdq.common.Logging;

namespace pdq.common
{
	public class Query : IQueryInternal
	{
		private readonly ILoggerProxy logger;
		private readonly ITransient transient;
		private readonly IAliasManager aliasManager;
		private IQueryContext context;

		private Query(
			ILoggerProxy logger,
			ITransient transient)
		{
			this.logger = logger;
			this.transient = transient;
			this.aliasManager = AliasManager.Create();

			Id = Guid.NewGuid();
			Status = QueryStatus.Empty;

			this.logger.Debug($"Query({Id}) :: Created as {Status}");
		}

        public Guid Id { get; private set; }

		public QueryStatus Status { get; private set; }

		IAliasManager IQueryInternal.AliasManager => this.aliasManager;

        ITransient IQueryInternal.Transient => this.transient;

        IQueryContext IQueryInternal.Context => this.context;

        string IQueryInternal.GetHash() => this.Id.ToString("N");

		public static IQuery Create(ILoggerProxy logger, ITransient transient) => new Query(logger, transient);

		void IQueryInternal.SetContext(IQueryContext context) => this.context = context;

		public void Dispose()
		{
			Dispose(true);
            GC.SuppressFinalize(this);
        }

		protected virtual void Dispose(bool disposing)
        {
			if (!disposing) return;
			this.logger.Debug($"Query({Id} :: Disposing({disposing})");
            this.aliasManager.Dispose();
        }
    }
}

