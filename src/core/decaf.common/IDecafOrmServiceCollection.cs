using System;
using decaf.common.Connections;
using Microsoft.Extensions.DependencyInjection;

namespace decaf.common
{
    public interface IDecafOrmServiceCollection : IServiceCollection
    {
        /// <summary>
        /// Specify a connection details implementation to use, this assumes that the connection is able to get its details from a configuration source.
        /// </summary>
        /// <param name="lifetime">The lifetime of the connection details. <see cref="ServiceLifetime"/></param>
        /// <typeparam name="TInterface">The connection details interface to use, either <see cref="IConnectionDetails"/>, or a sub-interface thereof.</typeparam>
        /// <typeparam name="TImplementation">The implementation type to use, must inherit from <see cref="TInterface"/>.</typeparam>
        /// <returns>The current <see cref="IServiceCollection"/> in use.</returns>
        IServiceCollection WithConnection<TInterface, TImplementation>(ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TInterface : class, IConnectionDetails
            where TImplementation : class, TInterface;

        /// <summary>
        /// Provide a connection details implementation and instance to use. Note that this will be registered as a singleton.
        /// </summary>
        /// <typeparam name="T">The type of connection details to register, this must inherit from <see cref="IConnectionDetails"/>.</typeparam>
        /// <param name="connectionDetails">The instance of <see cref="T"/> to use, note that it must inherit from <see cref="T"/></param>
        /// <returns>The current <see cref="IServiceCollection"/> in use.</returns>
        IServiceCollection WithConnection<T>(T connectionDetails)
            where T : class, IConnectionDetails;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="lifetime">The lifetime of the connection details. <see cref="ServiceLifetime"/></param>
        /// <returns>The current <see cref="IServiceCollection"/> in use.</returns>
        IServiceCollection WithConnection<T>(Action<T> builder, ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where T : class, IConnectionDetails, new();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="lifetime">The lifetime of the connection details. <see cref="ServiceLifetime"/></param>
        /// <returns>The current <see cref="IServiceCollection"/> in use.</returns>
        IServiceCollection WithConnection<T>(Func<IServiceProvider, T> expression, ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where T : class, IConnectionDetails;
    }
}