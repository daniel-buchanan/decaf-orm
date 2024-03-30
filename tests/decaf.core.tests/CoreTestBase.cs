using System;
using decaf.common;
using decaf.common.Connections;
using decaf.common.Logging;
using decaf.tests.common.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace decaf.core_tests;

public class CoreTestBase
{
    protected readonly IServiceProvider provider;
    protected readonly Mock<ILoggerProxy> logger = new Mock<ILoggerProxy>();
    
    public CoreTestBase()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ILoggerProxy>(logger.Object);
        services.AddDecaf(o =>
        {
            o.TrackUnitsOfWork();
            o.OverrideDefaultLogLevel(LogLevel.Debug);
            o.UseMockDatabase();
        })
            .WithConnection<IConnectionDetails>(new MockConnectionDetails());

        provider = services.BuildServiceProvider();
    }
}