using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace pdq.core_tests.Helpers
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection Replace<T, TReplacement>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where T : class
            where TReplacement : class, T
		{
			var existing = services.FirstOrDefault(s => s.ServiceType == typeof(T));

			if (existing != null)
				services.Remove(existing);

			if(lifetime == ServiceLifetime.Singleton)
				services.AddSingleton<T, TReplacement>();

			if(lifetime == ServiceLifetime.Scoped)
                services.AddScoped<T, TReplacement>();

			if(lifetime == ServiceLifetime.Transient)
                services.AddTransient<T, TReplacement>();

            return services;
		}
	}
}

