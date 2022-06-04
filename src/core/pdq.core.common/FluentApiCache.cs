using System.Linq;
using System.Collections.Generic;
using pdq.core.common.Logging;

namespace pdq.core.common
{
	public class FluentApiCache : IFluentApiCache
	{
        private readonly Dictionary<string, IFluentApi> cache;
		private readonly ILoggerProxy logger;

        public FluentApiCache(ILoggerProxy logger)
		{
			this.logger = logger;
            this.cache = new Dictionary<string, IFluentApi>();
		}

        public IReadOnlyList<string> KnownInstances => this.cache.Keys.ToList().AsReadOnly();

        public T Get<T>() where T : IFluentApi, new()
        {
			var instance = new T();
			var key = instance.GetHash();

			this.logger.Debug($"IFluentApiCache :: Get<> Looking for {key}");
			if (this.cache.TryGetValue(key, out var t))
			{
				this.logger.Debug($"IFluentApiCache :: Get<> found instance for {key}");
				instance.Dispose();
				return (T)t;
			}

			this.logger.Debug($"IFluentApiCache :: Get<> created instance for {key}");
			this.cache.Add(key, instance);
			return instance;
        }
    }
}

