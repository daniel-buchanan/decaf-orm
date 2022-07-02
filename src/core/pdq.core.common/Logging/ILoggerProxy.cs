using System;
namespace pdq.common.Logging
{
	/// <summary>
    /// This is the proxy for whatever logger is in use.
    /// If the default configuration is not used it will delegate to whatever
    /// logger you configure.
    /// </summary>
	public interface ILoggerProxy
	{
		/// <summary>
        /// Log a debugging message.<br/>
        /// This will be logged as DEBUG.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <example>logger.Debug("hello world");</example>
		void Debug(string message);

		/// <summary>
        /// Log a warning message.<br/>
        /// This will be logged as WARN.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <example>logger.Warning("hello world");</example>
		void Warning(string message);

        /// <summary>
        /// Log an error message.<br/>
        /// This will be logged as ERROR.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <example>logger.Error("hello world");</example>
		void Error(string message);

        /// <summary>
        /// Log an error message with an exception.<br/>
        /// This will be logged as ERROR.
        /// </summary>
        /// <param name="ex">The exception to log alongside the message.</param>
        /// <param name="message">The message to log.</param>
        /// <example>logger.Error(ex, "hello world");</example>
		void Error(Exception ex, string message);

        /// <summary>
        /// Log an informational message.<br/>
        /// This will be logged as INFO.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <example>logger.Information("hello world");</example>
		void Information(string message);
	}
}

