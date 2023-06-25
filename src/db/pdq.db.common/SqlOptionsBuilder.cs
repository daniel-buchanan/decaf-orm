using System;
using Microsoft.Extensions.Configuration;
using pdq.common.Connections;
using pdq.common.Options;

namespace pdq.db.common
{
	public abstract class SqlOptionsBuilder<TOptions, TSelf, TConnectionDetails> :
        OptionsBuilder<TOptions>,
        ISqlOptionsBuilder<TOptions, TSelf, TConnectionDetails>
        where TOptions : class, IDatabaseOptionsExtensions, new()
        where TSelf : class
        where TConnectionDetails : IConnectionDetails
    {

        /// <inheritdoc/>
        public TSelf WithConnectionDetails(TConnectionDetails connectionDetails)
            => ConfigureProperty(nameof(IDatabaseOptions.ConnectionDetails), connectionDetails);

        /// <inheritdoc/>
        public TSelf WithConnectionDetails(Func<IConfiguration, TConnectionDetails> configurer)
            => ConfigureProperty(
                nameof(IDatabaseOptionsExtensions.ConnectionDetailsConfigurationFactory),
                configurer);

        /// <inheritdoc/>
        public TSelf WithConnectionDetails(Func<IServiceProvider, TConnectionDetails> factory)
            => ConfigureProperty(
                nameof(IDatabaseOptionsExtensions.ConnectionDetailsServiceProviderFactory),
                factory);

        /// <inheritdoc/>
        public abstract TSelf WithConnectionString(string connectionString);

        /// <inheritdoc/>
        protected new TSelf ConfigureProperty<TValue>(string name, TValue value)
        {
            base.ConfigureProperty(name, value);
            return this as TSelf;
        }
    }
}

