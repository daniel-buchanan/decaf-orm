using System;
using Microsoft.Extensions.DependencyInjection;
using pdq.common.Connections;

namespace pdq.common
{
    public interface IPdqServiceCollection : IServiceCollection
    {
        /// <summary>
        /// Specify a connection details implementation to use, this assumes that the connection is able to get its details from a configuration source.
        /// </summary>
        /// <typeparam name="TInterface">The connection details interface to use, either <see cref="IConnectionDetails"/>, or a sub-interface thereof.</typeparam>
        /// <typeparam name="TImplementation">The implementation type to use, must inherit from <see cref="TInterface"/>.</typeparam>
        /// <returns>The current <see cref="IServiceCollection"/> in use.</returns>
        IServiceCollection WithConnection<TInterface, TImplementation>()
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
        /// <returns>The current <see cref="IServiceCollection"/> in use.</returns>
        IServiceCollection WithConnection<T>(Action<T> builder)
            where T : class, IConnectionDetails, new();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns>The current <see cref="IServiceCollection"/> in use.</returns>
        IServiceCollection WithConnection<T>(Func<IServiceProvider, T> expression)
            where T : class, IConnectionDetails;
    }
}