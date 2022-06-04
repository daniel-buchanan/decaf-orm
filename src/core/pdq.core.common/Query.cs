using System;
using pdq.core.common.Logging;

namespace pdq.core.common
{
	public class Query : IQuery
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

		public IQueryContext? Context { get; private set; }

		public static IQuery Create(ILoggerProxy logger, ITransient transient) => new Query(logger, transient);

		internal void SetContext(IQueryContext context)
        {
			Context = context;

			this.logger.Debug($"Query({Id}) :: Context set to {context.Id} - {context.Kind}");
        }

		internal T GetFluent<T>() where T : IFluentApi, new() => this.transient.GetFluent<T>();
    }
}

