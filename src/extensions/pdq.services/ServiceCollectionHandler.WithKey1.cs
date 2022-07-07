using Microsoft.Extensions.DependencyInjection;

namespace pdq.services
{
    public class ServiceCollectionHandler<TEntity, TKey> :
        IServiceCollectionHandler<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        private readonly IServiceCollection services;

        private ServiceCollectionHandler(IServiceCollection services)
            => this.services = services;

        public static IServiceCollectionHandler<TEntity, TKey> Create(IServiceCollection services)
            => new ServiceCollectionHandler<TEntity, TKey>(services);

        private IServiceCollection Add(ServiceLifetime lifetime)
        {
            this.services.Add(new ServiceDescriptor(typeof(IService<TEntity, TKey>), typeof(Service<TEntity, TKey>), lifetime));
            this.services.Add(new ServiceDescriptor(typeof(IQuery<TEntity, TKey>), typeof(Query<TEntity, TKey>), lifetime));
            this.services.Add(new ServiceDescriptor(typeof(ICommand<TEntity, TKey>), typeof(Command<TEntity, TKey>), lifetime));
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

