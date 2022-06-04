using System;
using pdq.common.Logging;

namespace pdq
{
	public class PdqOptions
	{
		internal LogLevel DefaultLogLevel { get; private set; } = LogLevel.Error;

		internal bool TrackTransients { get; private set; } = false;


		public void OverrideDefaultLogLevel(LogLevel level) => DefaultLogLevel = level;

		public void EnableTransientTracking() => TrackTransients = true;
	}
}

