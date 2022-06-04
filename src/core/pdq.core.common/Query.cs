using System;
using pdq.common.Logging;

namespace pdq.common
{
	public class Query : IQuery, IQueryInternal
	{
		private readonly ILoggerProxy logger;
		private readonly ITransient transient;

		private Query(
			ILoggerProxy logger,
			ITransient transient)
		{
			this.logger = logger;
			this.transient = transient;

			Id = Guid.NewGuid();
			Status = QueryStatus.Empty;

			this.logger.Debug($"Query({Id}) :: Created as {Status}");
		}

        public Guid Id { get; private set; }

		public QueryStatus Status { get; private set; }

        public static IQuery Create(ILoggerProxy logger, ITransient transient) => new Query(logger, transient);

		string IQueryInternal.GetHash() => this.Id.ToString("N");
    }
}

