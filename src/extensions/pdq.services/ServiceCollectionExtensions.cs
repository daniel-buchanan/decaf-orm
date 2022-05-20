using System;
using Microsoft.Extensions.DependencyInjection;

namespace pdq.services
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddScopedService<TEntity>(this IServiceCollection services)
			where TEntity : IEntity
        {
			services.AddScoped<IService<TEntity>>();
			return services;
        }

		public static IServiceCollection AddTransientService<TEntity>(this IServiceCollection services)
			where TEntity : IEntity
		{
			services.AddTransient<IService<TEntity>>();
			return services;
		}

		public static IServiceCollection AddSingletonService<TEntity>(this IServiceCollection services)
			where TEntity : IEntity
		{
			services.AddSingleton<IService<TEntity>>();
			return services;
		}
	}
}

