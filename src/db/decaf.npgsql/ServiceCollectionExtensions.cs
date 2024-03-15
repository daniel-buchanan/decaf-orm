using System;
using decaf.common;
using decaf.common.Options;
using decaf.db.common;

namespace decaf.npgsql
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Use PostgreSQL (Npgsql as the client) with the default configuration and options.
        /// </summary>
        /// <param name="services">The <see cref="IDecafOrmServiceCollection"/> to use.</param>
        public static IDecafOrmServiceCollection UseNpgsql(this IDecafOrmServiceCollection services)
            => UseNpgsql(services, new NpgsqlOptions());

        /// <summary>
        /// Use PostgreSQL (Npgsql as the client) with a builder for the Postgres options.
        /// </summary>
        /// <param name="services">The <see cref="IDecafOrmServiceCollection"/> to use.</param>
        /// <param name="builder">An <see cref="Action"/> to configure the <see cref="NpgsqlOptions"/>.</param>
        public static IDecafOrmServiceCollection UseNpgsql(
            this IDecafOrmServiceCollection services,
            Action<INpgsqlOptionsBuilder> builder)
        {
            var options = new NpgsqlOptionsBuilder();
            builder(options);
            return UseNpgsql(services, options.Build());
        }

        /// <summary>
        /// Use PostgreSQL (Npgsql as the client) with a provided <see cref="NpgsqlOptions"/>.
        /// </summary>
        /// <param name="services">The <see cref="IDecafOptionsBuilder"/> to use.</param>
        /// <param name="options">The <see cref="NpgsqlOptions"/> to use.</param>
        public static IDecafOrmServiceCollection UseNpgsql(
            this IDecafOrmServiceCollection services,
            NpgsqlOptions options)
        {
            services.UseDbImplementation<NpgsqlImplementationFactory>(options);
            return services;
        }
    }
}