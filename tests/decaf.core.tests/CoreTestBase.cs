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
        provider = Build(b => b.Noop());
    }

    protected IServiceProvider Build(Action<IMockDbOptionsBuilder> method, bool swallowExceptions = false)
    {
        var services = new ServiceCollection();
        services.AddSingleton<ILoggerProxy>(logger.Object);
        services.AddDecaf(o =>
            {
                o.TrackUnitsOfWork();
                o.OverrideDefaultLogLevel(LogLevel.Debug);
                o.UseMockDatabase(method);
                if (swallowExceptions) o.SwallowTransactionExceptions();
            })
            .WithConnection<IConnectionDetails>(new MockConnectionDetails());
        return services.BuildServiceProvider();
    }
}