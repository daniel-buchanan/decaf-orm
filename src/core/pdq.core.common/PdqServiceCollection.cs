using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using pdq.common.Connections;

namespace pdq.common
{
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
        public IServiceCollection WithConnection<T>(
            Action<T> builder,
            ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where T : class, IConnectionDetails, new()
            => WithConnection(
                p =>
                {
                    var connectionDetails = new T();
                    builder(connectionDetails);
                    return connectionDetails;
                }, lifetime); 

        /// <inheritdoc/>
        public IServiceCollection WithConnection<T>(
            Func<IServiceProvider, T> expression,
            ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where T : class, IConnectionDetails
        {
            Add(new ServiceDescriptor(typeof(T), expression, lifetime));
            return this;
        }

        /// <inheritdoc/>
        public IServiceCollection WithConnection<TInterface, TImplementation>(
            ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TInterface : class, IConnectionDetails
            where TImplementation : class, TInterface
        {
            Add(new ServiceDescriptor(typeof(TInterface), typeof(TImplementation), lifetime));
            return this;
        }

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