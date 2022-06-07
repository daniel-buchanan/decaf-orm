using System;
using pdq.common.Logging;

namespace pdq
{
	public class PdqOptions
	{
		public LogLevel DefaultLogLevel { get; private set; } = LogLevel.Error;

		public bool TrackTransients { get; private set; } = false;

		internal Type LoggerProxyType { get; private set; } = typeof(DefaultLogger);

		internal Type SqlFactoryType { get; private set; }

		public void OverrideDefaultLogLevel(LogLevel level) => DefaultLogLevel = level;

		public void EnableTransientTracking() => TrackTransients = true;
	}
}

