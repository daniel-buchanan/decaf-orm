using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using pdq.common.Connections;
using pdq.common.Exceptions;
using pdq.db.common.Exceptions;

namespace pdq.db.common
{
	public class DatabaseOptions : IDatabaseOptionsExtensions
	{
        /// <inheritdoc/>
        public Func<IServiceProvider, IConnectionDetails> ConnectionDetailsServiceProviderFactory { get; protected set; }

        /// <inheritdoc/>
        public Func<IConfiguration, IConnectionDetails> ConnectionDetailsConfigurationFactory { get; protected set; }

        /// <inheritdoc/>
        public IConnectionDetails ConnectionDetails { get; protected set; }

        /// <inheritdoc/>
        public bool QuotedIdentifiers { get; protected set; }

        /// <inheritdoc/>
        public IConnectionDetails GetConnectionDetails(IServiceProvider provider)
        {
            if (ConnectionDetails != null) return ConnectionDetails;

            if (ConnectionDetailsServiceProviderFactory != null &&
                provider == null)
            {
                throw new ServiceNotFoundException(nameof(IConnectionDetails));
            }

            var configuration = provider?.GetService<IConfiguration>();
            if (ConnectionDetailsConfigurationFactory != null &&
                configuration == null)
            {
                throw new ServiceNotFoundException(nameof(IConfiguration));
            }

            if (ConnectionDetailsConfigurationFactory != null)
                return ConnectionDetailsConfigurationFactory(configuration);

            if (ConnectionDetailsServiceProviderFactory != null)
                return ConnectionDetailsServiceProviderFactory(provider);

            throw new ShouldNeverOccurException("No way to get IConnectionDetails.");
        }
    }
}

