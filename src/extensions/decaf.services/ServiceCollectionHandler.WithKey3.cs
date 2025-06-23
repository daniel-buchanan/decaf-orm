using decaf.services;
using Microsoft.Extensions.DependencyInjection;

namespace decaf;

public class ServiceCollectionHandler<TEntity, TKey1, TKey2, TKey3> :
    IServiceCollectionHandler
    where TEntity : class, IEntity<TKey1, TKey2, TKey3>, new()
{
    private readonly IServiceCollection services;

    private ServiceCollectionHandler(IServiceCollection services)
        => this.services = services;

    public static IServiceCollectionHandler Create(IServiceCollection services)
        => new ServiceCollectionHandler<TEntity, TKey1, TKey2, TKey3>(services);

    private IServiceCollection Add(ServiceLifetime lifetime)
    {
        services.Add(new ServiceDescriptor(typeof(IService<TEntity, TKey1, TKey2, TKey3>), typeof(Service<TEntity, TKey1, TKey2, TKey3>), lifetime));
        services.Add(new ServiceDescriptor(typeof(IQuery<TEntity, TKey1, TKey2, TKey3>), typeof(Query<TEntity, TKey1, TKey2, TKey3>), lifetime));
        services.Add(new ServiceDescriptor(typeof(ICommand<TEntity, TKey1, TKey2, TKey3>), typeof(Command<TEntity, TKey1, TKey2, TKey3>), lifetime));
        return services;
    }

    /// <inheritdoc/>
    public IServiceCollection AsScoped() => Add(ServiceLifetime.Scoped);

    /// <inheritdoc/>
    public IServiceCollection AsSingleton() => Add(ServiceLifetime.Singleton);

    /// <inheritdoc/>
    public IServiceCollection AsTransient() => Add(ServiceLifetime.Transient);
}