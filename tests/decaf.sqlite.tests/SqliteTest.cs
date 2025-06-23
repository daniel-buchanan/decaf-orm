using System;
using decaf.common.Connections;
using Microsoft.Extensions.DependencyInjection;

namespace decaf.sqlite.tests;

public abstract class SqliteTest
{
    protected IServiceProvider provider;
    protected readonly IServiceCollection services;

    public SqliteTest(bool disableHeaderComments = true)
    {
        services = new ServiceCollection();
        services.AddDecaf(o =>
        {
            o.TrackUnitsOfWork()
                .OverrideDefaultLogLevel(LogLevel.Debug)
                .UseSqlite(options =>
                {
                    options.WithConnectionDetails(new SqliteConnectionDetails()
                    {
                        Authentication = new UsernamePasswordAuthentication("bob", "password")
                    });
                })
                .LazyInitialiseConnections();

            if(disableHeaderComments) o.DisableSqlHeaderComments();
        });
    }

    protected void BuildServiceProvider() => provider = services.BuildServiceProvider();
}