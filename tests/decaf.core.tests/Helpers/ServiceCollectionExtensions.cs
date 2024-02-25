using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace decaf.core_tests.Helpers
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection Replace<T, TReplacement>(this IServiceCollection services, ServiceLifetime? lifetime = null)
            where T : class
            where TReplacement : class, T
		{
			var existing = services.FirstOrDefault(s => s.ServiceType == typeof(T));

            if (lifetime == null)
                lifetime = existing.Lifetime;

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

        public static IServiceCollection Replace<T, TReplacement>(this IServiceCollection services, TReplacement instance)
            where T : class
            where TReplacement : class, T
        {
            var existing = services.FirstOrDefault(s => s.ServiceType == typeof(T));

            if (existing != null)
                services.Remove(existing);

            services.AddSingleton<T>(instance);

            return services;
        }

        public static IServiceCollection Replace<T, TReplacement>(this IServiceCollection services, Func<IServiceProvider, T> func, ServiceLifetime? lifetime = null)
            where T : class
            where TReplacement : class, T
        {
            var existing = services.FirstOrDefault(s => s.ServiceType == typeof(T));

            if (lifetime == null)
                lifetime = existing.Lifetime;

            if (existing != null)
                services.Remove(existing);

            if (lifetime == ServiceLifetime.Singleton)
                services.AddSingleton<T>(func);

            if (lifetime == ServiceLifetime.Scoped)
                services.AddScoped<T>(func);

            if (lifetime == ServiceLifetime.Transient)
                services.AddTransient<T>(func);

            return services;
        }
    }
}

