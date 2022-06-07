using System;
using pdq.common.Logging;

namespace pdq.common
{
	public class Query : IQuery, IQueryInternal
	{
		private readonly ILoggerProxy logger;
		private readonly ITransient transient;
		private readonly IAliasManager aliasManager;

		private Query(
			ILoggerProxy logger,
			ITransient transient)
		{
			this.logger = logger;
			this.transient = transient;
			this.aliasManager = new AliasManager();

			Id = Guid.NewGuid();
			Status = QueryStatus.Empty;

			this.logger.Debug($"Query({Id}) :: Created as {Status}");
		}

        public Guid Id { get; private set; }

		public QueryStatus Status { get; private set; }

		IAliasManager IQueryInternal.AliasManager => this.aliasManager;

        ITransient IQueryInternal.Transient => this.transient;

        string IQueryInternal.GetHash() => this.Id.ToString("N");

		public static IQuery Create(ILoggerProxy logger, ITransient transient) => new Query(logger, transient);
	}
}

