using System;
namespace pdq.core.Logging
{
	public interface ILoggerProxy
	{
		void Debug(string message);

		void Warning(string message);

		void Error(string message);

		void Error(Exception ex, string message);

		void Information(string message);
	}
}

