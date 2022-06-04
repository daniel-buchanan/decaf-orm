using System;

namespace pdq.common.Logging
{
	public class DefaultLogger : LoggerProxy
	{
		public DefaultLogger(LogLevel defaultLogLevel) : base(defaultLogLevel) { }

        protected override void WriteMessage(LogLevel level, string message)
        {
            var timestamp = DateTime.Now.ToString("yy-MM-dd hh:mm:ss:fff");
            Console.WriteLine($"[{timestamp}] :: {LogLevelToString(level)} :: {message}");
        }

        private string LogLevelToString(LogLevel level)
        {
            switch(level)
            {
                case LogLevel.Debug: return "DEBUG";
                case LogLevel.Warning: return "WARN ";
                case LogLevel.Information: return "ERROR";
                default: return "INFO ";
            }
        }
    }
}

