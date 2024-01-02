using System;
using pdq.common;
using pdq.common.Options;
using pdq.db.common;

namespace pdq.npgsql
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Use PostgreSQL (Npgsql as the client) with the default configuration and options.
        /// </summary>
        /// <param name="services">The <see cref="IPdqServiceCollection"/> to use.</param>
        public static IPdqServiceCollection UseNpgsql(this IPdqServiceCollection services)
            => UseNpgsql(services, new NpgsqlOptions());

        /// <summary>
        /// Use PostgreSQL (Npgsql as the client) with a builder for the Postgres options.
        /// </summary>
        /// <param name="services">The <see cref="IPdqServiceCollection"/> to use.</param>
        /// <param name="builder">An <see cref="Action"/> to configure the <see cref="NpgsqlOptions"/>.</param>
        public static IPdqServiceCollection UseNpgsql(
            this IPdqServiceCollection services,
            Action<INpgsqlOptionsBuilder> builder)
        {
            var options = new NpgsqlOptionsBuilder();
            builder(options);
            return UseNpgsql(services, options.Build());
        }

        /// <summary>
        /// Use PostgreSQL (Npgsql as the client) with a provided <see cref="NpgsqlOptions"/>.
        /// </summary>
        /// <param name="services">The <see cref="IPdqOptionsBuilder"/> to use.</param>
        /// <param name="options">The <see cref="NpgsqlOptions"/> to use.</param>
        public static IPdqServiceCollection UseNpgsql(
            this IPdqServiceCollection services,
            NpgsqlOptions options)
        {
            services.UseDbImplementation<NpgsqlImplementationFactory>(options);
            return services;
        }
    }
}