using System;
using Microsoft.Extensions.DependencyInjection;
using pdq.core.common.Logging;
using pdq.core.common;

namespace pdq.core
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddPdq(this IServiceCollection services)
        {
			var o = new PdqOptions();
			return AddPdq(services, o);
        }

		public static IServiceCollection AddPdq(this IServiceCollection services, Action<PdqOptions> options)
        {
			var o = new PdqOptions();
			options(o);
			return AddPdq(services, o);
        }

		public static IServiceCollection AddPdq(this IServiceCollection services, PdqOptions options)
        {
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddSingleton<ILoggerProxy>(new DefaultLogger(options.DefaultLogLevel));
			return services;
		}
	}
}

