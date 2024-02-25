using System;

namespace decaf.common.Logging
{
	public abstract class LoggerProxy : ILoggerProxy
	{
        private const string Prefix = "Pdq :: ";
        private readonly LogLevel defaultLogLevel;

        protected LoggerProxy(LogLevel defaultLogLevel)
        {
            this.defaultLogLevel = defaultLogLevel;
        }

        /// <inheritdoc/>
        public void Debug(string message)
        {
            if (defaultLogLevel > LogLevel.Debug) return;
            WriteMessage(LogLevel.Debug, Prefix + message);
        }

        /// <inheritdoc/>
        public void Warning(string message)
        {
            if (defaultLogLevel > LogLevel.Warning) return;
            WriteMessage(LogLevel.Warning, Prefix + message);
        }

        /// <inheritdoc/>
        public void Information(string message)
        {
            if (defaultLogLevel > LogLevel.Information) return;
            WriteMessage(LogLevel.Information, Prefix + message);
        }

        /// <inheritdoc/>
        public void Error(string message)
        {
            WriteMessage(LogLevel.Error, Prefix + message);
        }

        /// <inheritdoc/>
        public void Error(Exception ex, string message)
        {
            WriteMessage(LogLevel.Error, Prefix + message);
            WriteMessage(LogLevel.Error, ex.StackTrace);
        }

        protected abstract void WriteMessage(LogLevel level, string message);
    }
}

