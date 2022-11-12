using Microsoft.Extensions.DependencyInjection;
using pdq.services;

namespace pdq
{
    public class ServiceCollectionHandler<TEntity, TKey> :
        IServiceCollectionHandler
        where TEntity : class, IEntity<TKey>, new()
    {
        private readonly IServiceCollection services;

        private ServiceCollectionHandler(IServiceCollection services)
            => this.services = services;

        public static IServiceCollectionHandler Create(IServiceCollection services)
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

