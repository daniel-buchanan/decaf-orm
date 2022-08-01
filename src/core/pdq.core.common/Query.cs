using System;
using pdq.common.Logging;

namespace pdq.common
{
	public class Query : IQueryInternal
	{
		private readonly ILoggerProxy logger;
		private readonly ITransientInternal transient;
		private readonly IAliasManager aliasManager;
		private readonly PdqOptions options;
		private IQueryContext context;

		private Query(
			PdqOptions options,
			ILoggerProxy logger,
			ITransient transient)
		{
			this.logger = logger;
			this.transient = transient as ITransientInternal;
			this.aliasManager = AliasManager.Create();
			this.options = options;

			Id = Guid.NewGuid();
			Status = QueryStatus.Empty;

			this.logger.Debug($"Query({Id}) :: Created as {Status}");
		}

		/// <inheritdoc/>
        public Guid Id { get; private set; }

		/// <inheritdoc/>
		public QueryStatus Status { get; private set; }

		/// <inheritdoc/>
		IAliasManager IQueryInternal.AliasManager => this.aliasManager;

		/// <inheritdoc/>
		ITransient IQueryInternal.Transient => this.transient;

		/// <inheritdoc/>
		IQueryContext IQueryInternal.Context => this.context;

		/// <inheritdoc/>
		PdqOptions IQueryInternal.Options => this.options;

		/// <inheritdoc/>
		public ISqlFactory SqlFactory => this.transient.SqlFactory;

		/// <inheritdoc/>
		string IQueryInternal.GetHash() => this.Id.ToString("N");

		/// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        /// <param name="transient"></param>
        /// <returns></returns>
		public static IQuery Create(PdqOptions options, ILoggerProxy logger, ITransient transient)
			=> new Query(options, logger, transient);

		/// <inheritdoc/>
		void IQueryInternal.SetContext(IQueryContext context) => this.context = context;

		/// <inheritdoc/>
		public void Dispose()
		{
			Dispose(true);
            GC.SuppressFinalize(this);
        }

		/// <inheritdoc/>
		protected virtual void Dispose(bool disposing)
        {
			if (!disposing) return;
			this.logger.Debug($"Query({Id} :: Disposing({disposing})");
            this.aliasManager.Dispose();
        }
    }
}

