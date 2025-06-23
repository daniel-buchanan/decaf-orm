using System;
using decaf.common.Logging;
using Serilog;

namespace decaf.logging.serilog;

public class LoggerProxy : ILoggerProxy
{
    private readonly ILogger logger;

    public LoggerProxy(ILogger logger) 
        => this.logger = logger;

    public void Debug(string message)
        => logger.Debug(message);

    public void Warning(string message)
        => logger.Warning(message);

    public void Error(string message)
        => logger.Error(message);

    public void Error(Exception ex, string message)
        => logger.Error(ex, message);

    public void Information(string message)
        => logger.Information(message);
}