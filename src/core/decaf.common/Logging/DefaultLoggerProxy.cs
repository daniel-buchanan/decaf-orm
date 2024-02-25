using System;

namespace decaf.common.Logging
{
	public class DefaultLoggerProxy : LoggerProxy
    {
        private readonly IStdOutputWrapper output;

        public DefaultLoggerProxy(
            DecafOptions options,
            IStdOutputWrapper output) : base(options.DefaultLogLevel)
            => this.output = output;

        protected override void WriteMessage(LogLevel level, string message)
        {
            var timestamp = DateTime.Now.ToString("yy-MM-dd hh:mm:ss:fff");
            output.WriteOut($"[{timestamp}] :: {LogLevelToString(level)} :: {message}");
        }

        private string LogLevelToString(LogLevel level)
        {
            switch(level)
            {
                case LogLevel.Debug: return "DEBUG";
                case LogLevel.Warning: return "WARN ";
                case LogLevel.Error: return "ERROR";
                default: return "INFO ";
            }
        }
    }
}

