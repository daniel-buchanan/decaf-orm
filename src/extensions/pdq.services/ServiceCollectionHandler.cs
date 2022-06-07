using System;
using Microsoft.Extensions.DependencyInjection;

namespace pdq.services
{
	public interface IServiceCollectionHandler<TEntity>
		where TEntity : IEntity
	{
		IServiceCollection AsScoped();
		IServiceCollection AsSingleton();
		IServiceCollection AsTransient();
	}

	public interface IServiceCollectionHandler<TEntity, TKey> : IServiceCollectionHandler<TEntity>
		where TEntity : IEntity<TKey> { }

    public class ServiceCollectionHandler<TEntity> : IServiceCollectionHandler<TEntity>
        where TEntity : IEntity
    {
        private readonly IServiceCollection services;

        public ServiceCollectionHandler(IServiceCollection services)
        {
            this.services = services;
        }

        public IServiceCollection AsScoped()
        {
            this.services.AddScoped<IService<TEntity>>();
            this.services.AddScoped<IQuery<TEntity>>();
            this.services.AddScoped<ICommand<TEntity>>();
            return this.services;
        }

        public IServiceCollection AsSingleton()
        {
            this.services.AddSingleton<IService<TEntity>>();
            this.services.AddSingleton<IQuery<TEntity>>();
            this.services.AddSingleton<ICommand<TEntity>>();
            return this.services;
        }

        public IServiceCollection AsTransient()
        {
            this.services.AddTransient<IService<TEntity>>();
            this.services.AddTransient<IQuery<TEntity>>();
            this.services.AddTransient<ICommand<TEntity>>();
            return this.services;
        }
    }

    public class ServiceCollectionHandler<TEntity, TKey> : ServiceCollectionHandler<TEntity>, IServiceCollectionHandler<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
        public ServiceCollectionHandler(IServiceCollection services)
            : base(services) { }
    }
}

