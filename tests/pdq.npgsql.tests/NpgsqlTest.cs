using System;
using Microsoft.Extensions.DependencyInjection;
using pdq.common.Connections;

namespace pdq.npgsql.tests
{
	public abstract class NpgsqlTest
	{
        protected IServiceProvider provider;
        protected readonly IServiceCollection services;

		public NpgsqlTest(bool disableHeaderComments = true)
		{
            services = new ServiceCollection();
            services.AddPdq(o =>
            {
                o.TrackUnitsOfWork()
                    .OverrideDefaultLogLevel(LogLevel.Debug)
                    .UseNpgsql(options =>
                    {
                        options.WithConnectionDetails(new NpgsqlConnectionDetails()
                        {
                            Authentication = new UsernamePasswordAuthentication("bob", "password")
                        });
                    });

                if(disableHeaderComments) o.DisableSqlHeaderComments();
            });
        }

        protected void BuildServiceProvider() => provider = services.BuildServiceProvider();
	}
}

