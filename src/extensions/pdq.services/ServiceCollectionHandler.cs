﻿using Microsoft.Extensions.DependencyInjection;

namespace pdq.services
{
    public class ServiceCollectionHandler<TEntity> :
        IServiceCollectionHandler<TEntity>
        where TEntity : class, IEntity
    {
        private readonly IServiceCollection services;

        private ServiceCollectionHandler(IServiceCollection services)
            => this.services = services;

        public static IServiceCollectionHandler<TEntity> Create(IServiceCollection services)
            => new ServiceCollectionHandler<TEntity>(services);

        private IServiceCollection Add(ServiceLifetime lifetime)
        {
            this.services.Add(new ServiceDescriptor(typeof(IService<TEntity>), typeof(Service<TEntity>), lifetime));
            this.services.Add(new ServiceDescriptor(typeof(IQuery<TEntity>), typeof(Query<TEntity>), lifetime));
            this.services.Add(new ServiceDescriptor(typeof(ICommand<TEntity>), typeof(Command<TEntity>), lifetime));
            return this.services;
        }

        /// <inheritdoc/>
        public IServiceCollection AsScoped() => Add(ServiceLifetime.Scoped);

        /// <inheritdoc/>
        public IServiceCollection AsSingleton() => Add(ServiceLifetime.Singleton);

        /// <inheritdoc/>
        public IServiceCollection AsTransient() => Add(ServiceLifetime.Transient);
    }
}

