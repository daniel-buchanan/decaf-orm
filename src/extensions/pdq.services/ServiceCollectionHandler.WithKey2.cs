using Microsoft.Extensions.DependencyInjection;
using pdq.services;

namespace pdq
{
    public class ServiceCollectionHandler<TEntity, TKey1, TKey2> :
        IServiceCollectionHandler
        where TEntity : class, IEntity<TKey1, TKey2>, new()
    {
        private readonly IServiceCollection services;

        private ServiceCollectionHandler(IServiceCollection services)
            => this.services = services;

        public static IServiceCollectionHandler Create(IServiceCollection services)
            => new ServiceCollectionHandler<TEntity, TKey1, TKey2>(services);

        private IServiceCollection Add(ServiceLifetime lifetime)
        {
            this.services.Add(new ServiceDescriptor(typeof(IService<TEntity, TKey1, TKey2>), typeof(Service<TEntity, TKey1, TKey2>), lifetime));
            this.services.Add(new ServiceDescriptor(typeof(IQuery<TEntity, TKey1, TKey2>), typeof(Query<TEntity, TKey1, TKey2>), lifetime));
            this.services.Add(new ServiceDescriptor(typeof(ICommand<TEntity, TKey1, TKey2>), typeof(Command<TEntity, TKey1, TKey2>), lifetime));
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

