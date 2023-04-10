using System;
using Microsoft.Extensions.DependencyInjection;

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
                o.EnableTransientTracking();
                o.OverrideDefaultLogLevel(LogLevel.Debug);
                if(disableHeaderComments) o.DisableSqlHeaderComments();
                o.UseNpgsql(options =>
                {
                    options.WithConnectionDetails(new NpgsqlConnectionDetails());
                });
            });
        }

        protected void BuildServiceProvider() => provider = services.BuildServiceProvider();
	}
}

