using System;

namespace pdq.common.Logging
{
	public abstract class LoggerProxy : ILoggerProxy
	{
        private const string Prefix = "Pdq :: ";
        private readonly LogLevel defaultLogLevel;

        protected LoggerProxy(LogLevel defaultLogLevel)
        {
            this.defaultLogLevel = defaultLogLevel;
        }

        public void Debug(string message)
        {
            if (defaultLogLevel > LogLevel.Debug) return;
            WriteMessage(LogLevel.Debug, Prefix + message);
        }

        public void Warning(string message)
        {
            if (defaultLogLevel > LogLevel.Warning) return;
            WriteMessage(LogLevel.Warning, Prefix + message);
        }

        public void Information(string message)
        {
            if (defaultLogLevel > LogLevel.Information) return;
            WriteMessage(LogLevel.Information, Prefix + message);
        }

        public void Error(string message)
        {
            WriteMessage(LogLevel.Error, Prefix + message);
        }

        public void Error(Exception ex, string message)
        {
            WriteMessage(LogLevel.Error, Prefix + message);
            WriteMessage(LogLevel.Error, ex.StackTrace);
        }

        protected abstract void WriteMessage(LogLevel level, string message);
    }
}

