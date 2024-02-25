using decaf.services;
using Microsoft.Extensions.DependencyInjection;

namespace decaf
{
    public class ServiceCollectionHandler<TEntity> :
        IServiceCollectionHandler
        where TEntity : class, IEntity, new()
    {
        private readonly IServiceCollection services;

        private ServiceCollectionHandler(IServiceCollection services)
            => this.services = services;

        public static IServiceCollectionHandler Create(IServiceCollection services)
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

