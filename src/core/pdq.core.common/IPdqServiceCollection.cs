using System;
using System.Collections;
using System.Collections.Generic;
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

    public class PdqServiceCollection : IPdqServiceCollection
    {
        private readonly IServiceCollection services;

        private PdqServiceCollection(IServiceCollection services)
            => this.services = services;

        public static IPdqServiceCollection Create(IServiceCollection services)
            => new PdqServiceCollection(services);
        
        /// <inheritdoc/>
        public IServiceCollection WithConnection<T>(T connectionDetails)
            where T : class, IConnectionDetails
            => this.AddSingleton<T>(connectionDetails);

        /// <inheritdoc/>
        public IServiceCollection WithConnection<T>(Action<T> builder)
            where T : class, IConnectionDetails, new()
        {
            var options = new T();
            builder(options);
            return this.AddSingleton<T>(options);
        }

        /// <inheritdoc/>
        public IServiceCollection WithConnection<T>(Func<IServiceProvider, T> expression)
            where T : class, IConnectionDetails
            => this.AddScoped<T>(expression);

        /// <inheritdoc/>
        public IServiceCollection WithConnection<TInterface, TImplementation>()
            where TInterface : class, IConnectionDetails
            where TImplementation : class, TInterface
            => this.AddScoped<TInterface, TImplementation>();

        /// <inheritdoc/>
        public IEnumerator<ServiceDescriptor> GetEnumerator() => services.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => services.GetEnumerator();

        /// <inheritdoc/>
        public void Add(ServiceDescriptor item) => services.Add(item);

        /// <inheritdoc/>
        public void Clear() => services.Clear();

        /// <inheritdoc/>
        public bool Contains(ServiceDescriptor item) => services.Contains(item);

        /// <inheritdoc/>
        public void CopyTo(ServiceDescriptor[] array, int arrayIndex) => services.CopyTo(array, arrayIndex);

        /// <inheritdoc/>
        public bool Remove(ServiceDescriptor item) => services.Remove(item);

        /// <inheritdoc/>
        public int Count => services.Count;
        
        /// <inheritdoc/>
        public bool IsReadOnly => services.IsReadOnly;

        /// <inheritdoc/>
        public int IndexOf(ServiceDescriptor item) => services.IndexOf(item);

        /// <inheritdoc/>
        public void Insert(int index, ServiceDescriptor item) => services.Insert(index, item);

        /// <inheritdoc/>
        public void RemoveAt(int index) => services.RemoveAt(index);

        /// <inheritdoc/>
        public ServiceDescriptor this[int index]
        {
            get => services[index];
            set => services[index] = value;
        }
    }
}