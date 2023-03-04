using System;
using pdq.common.Logging;
using pdq.common.Utilities;

namespace pdq.common
{
	public class QueryFramework : IQueryContainerInternal
	{
		private readonly ILoggerProxy logger;
		private readonly ITransientInternal transient;
		private readonly IAliasManager aliasManager;
		private readonly PdqOptions options;
		private readonly IHashProvider hashProvider;
		private IQueryContext context;

		private QueryFramework(
			PdqOptions options,
			ILoggerProxy logger,
			ITransient transient,
			IHashProvider hashProvider)
		{
			this.logger = logger;
			this.transient = transient as ITransientInternal;
			this.aliasManager = AliasManager.Create();
			this.options = options;
			this.hashProvider = hashProvider;

			Id = Guid.NewGuid();
			Status = QueryStatus.Empty;

			this.logger.Debug($"Query({Id}) :: Created as {Status}");
		}

		/// <inheritdoc/>
        public Guid Id { get; private set; }

		/// <inheritdoc/>
		public QueryStatus Status { get; private set; }

		/// <inheritdoc/>
		IAliasManager IQueryContainerInternal.AliasManager => this.aliasManager;

		/// <inheritdoc/>
		ITransient IQueryContainerInternal.Transient => this.transient;

		/// <inheritdoc/>
		IQueryContext IQueryContainerInternal.Context => this.context;

        /// <inheritdoc/>
        IHashProvider IQueryContainerInternal.HashProvider => this.hashProvider;

        /// <inheritdoc/>
        PdqOptions IQueryContainerInternal.Options => this.options;

		/// <inheritdoc/>
		public ISqlFactory SqlFactory => this.transient.SqlFactory;

        /// <inheritdoc/>
        public ILoggerProxy Logger => this.logger;

        /// <inheritdoc/>
        string IQueryContainerInternal.GetHash() => this.context.GetHash();

		/// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        /// <param name="transient"></param>
        /// <returns></returns>
		public static IQueryContainer Create(
			PdqOptions options,
			ILoggerProxy logger,
			ITransient transient,
			IHashProvider hashProvider)
			=> new QueryFramework(options, logger, transient, hashProvider);

		/// <inheritdoc/>
		void IQueryContainerInternal.SetContext(IQueryContext context) => this.context = context;

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

