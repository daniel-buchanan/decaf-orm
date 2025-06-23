using System;

namespace decaf.common.Logging;

public class DefaultLoggerProxy(
    DecafOptions options,
    IStdOutputWrapper output) : LoggerProxy(options.DefaultLogLevel)
{
    protected override void WriteMessage(LogLevel level, string message)
    {
        var timestamp = DateTime.Now.ToString("yy-MM-dd hh:mm:ss:fff");
        output.WriteOut($"[{timestamp}] :: {LogLevelToString(level)} :: {message}");
    }

    private static string LogLevelToString(LogLevel level)
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