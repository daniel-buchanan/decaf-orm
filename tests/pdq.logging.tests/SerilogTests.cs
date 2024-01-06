using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using pdq.common.Connections;
using pdq.common.Logging;
using pdq.logging.serilog;
using pdq.tests.common.Mocks;
using Serilog;
using Xunit;

namespace pdq.logging.tests;

public class SerilogTests
{
    private readonly Mock<ILogger> logger;
    private readonly IServiceProvider provider;
    
    public SerilogTests()
    {
        var services = new ServiceCollection();
        services.AddPdq(o =>
            {
                o.TrackUnitsOfWork()
                    .OverrideDefaultLogLevel(LogLevel.Debug)
                    .UseMockDatabase()
                    .UseSerilog();
            })
            .WithConnection<IConnectionDetails, MockConnectionDetails>();

        logger = new Mock<ILogger>();
        services.AddScoped<ILogger>(p => logger.Object);
        provider = services.BuildServiceProvider();
    }
    
    [Fact]
    public void LoggerProxyIsRegistered()
    {
        // Act
        var proxy = provider.GetRequiredService<ILoggerProxy>();

        // Assert
        proxy.Should().NotBeNull();
    }
    
    [Theory]
    [MemberData(nameof(LoggerOperations))]
    public void LoggerProxyCallsLogger(Action<ILoggerProxy, string> method)
    {
        // Arrange
        var message = "hello world";
        var proxy = provider.GetRequiredService<ILoggerProxy>();
        
        // Act
        method(proxy, message);

        // Assert
        logger.VerifyAll();
    }

    [Fact]
    public void LoggerProxyCallsError()
    {
        // Arrange
        var message = "hello world";
        var proxy = provider.GetRequiredService<ILoggerProxy>();
        
        // Act
        proxy.Error(new Exception(message), message);
        
        // Assert
        logger.Verify(l => l.Error(It.IsAny<Exception>(), It.IsAny<string>()));
    }

    public static IEnumerable<object[]> LoggerOperations
    {
        get
        {
            yield return new object[] { (ILoggerProxy p, string m) => p.Debug(m) };
            yield return new object[] { (ILoggerProxy p, string m) => p.Information(m) };
            yield return new object[] { (ILoggerProxy p, string m) => p.Warning(m) };
            yield return new object[] { (ILoggerProxy p, string m) => p.Error(m) };
        }
    }
}