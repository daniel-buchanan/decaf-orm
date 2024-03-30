using System;
using decaf.common;
using decaf.common.Connections;
using decaf.tests.common.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace decaf.core_tests;

public class CoreTestBase
{
    protected readonly IServiceProvider provider;
    
    public CoreTestBase()
    {
        var services = new ServiceCollection();
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