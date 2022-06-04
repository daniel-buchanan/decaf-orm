using System;
namespace pdq.common.Logging
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

