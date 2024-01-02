using System;
using Microsoft.Extensions.DependencyInjection;
using pdq.common.Connections;

namespace pdq.sqlserver.tests
{
	public abstract class SqlServerTest
	{
        protected IServiceProvider provider;
        protected readonly IServiceCollection services;

		public SqlServerTest(bool disableHeaderComments = true)
		{
            services = new ServiceCollection();
            services.AddPdq(o =>
            {
                o.TrackUnitsOfWork()
                    .OverrideDefaultLogLevel(LogLevel.Debug)
                    .UseSqlServer(options =>
                    {
                        options.WithConnectionDetails(new SqlServerConnectionDetails()
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

