using System;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using pdq.common;
using pdq.common.Connections;

namespace pdq
{
    public interface IPdqServiceCollection : IServiceCollection
    {
        IServiceCollection WithConnection<TInterface, TImplementation>()
            where TInterface : class, IConnectionDetails
            where TImplementation : class, TInterface;

        IServiceCollection WithConnection<T>(T connectionDetails)
            where T : class, IConnectionDetails;

        IServiceCollection WithConnection<T>(Action<T> builder)
            where T : class, IConnectionDetails, new();

        IServiceCollection WithConnection<T>(Func<IServiceProvider, T> expression)
            where T : class, IConnectionDetails;
    }

    public class PdqServiceCollection : ServiceCollection, IPdqServiceCollection
    {
        public IServiceCollection WithConnection<T>(T connectionDetails)
            where T : class, IConnectionDetails
            => this.AddSingleton<T>(connectionDetails);

        public IServiceCollection WithConnection<T>(Action<T> builder)
            where T : class, IConnectionDetails, new()
        {
            var options = new T();
            builder(options);
            return this.AddSingleton<T>(options);
        }

        public IServiceCollection WithConnection<T>(Func<IServiceProvider, T> expression)
            where T : class, IConnectionDetails
            => this.AddScoped<T>(expression);

        public IServiceCollection WithConnection<TInterface, TImplementation>()
            where TInterface : class, IConnectionDetails
            where TImplementation : class, TInterface
            => this.AddScoped<TInterface, TImplementation>();
    }
}