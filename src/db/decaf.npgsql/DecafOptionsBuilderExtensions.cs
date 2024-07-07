using System;
using decaf.common.Options;
using decaf.db.common;

namespace decaf.npgsql
{
    public static partial class DecafOptionsBuilderExtensions
    {
        /// <summary>
        /// Use PostgreSQL (Npgsql as the client) with the default configuration and options.
        /// </summary>
        /// <param name="builder">The <see cref="IDecafOptionsBuilder"/> to use.</param>
        public static IDecafOptionsBuilder UseNpgsql(this IDecafOptionsBuilder builder)
            => UseNpgsql(builder, new NpgsqlOptions());

        /// <summary>
        /// Use PostgreSQL (Npgsql as the client) with a builder for the Postgres options.
        /// </summary>
        /// <param name="optionsBuilder">The <see cref="IDecafOptionsBuilder"/> to use.</param>
        /// <param name="builder">An <see cref="Action{T}"/> to configure the <see cref="NpgsqlOptions"/>.</param>
        public static IDecafOptionsBuilder UseNpgsql(
            this IDecafOptionsBuilder optionsBuilder,
            Action<INpgsqlOptionsBuilder> builder)
        {
            var options = new NpgsqlOptionsBuilder();
            builder(options);
            return UseNpgsql(optionsBuilder, options.Build());
        }

        /// <summary>
        /// Use PostgreSQL (Npgsql as the client) with a provided <see cref="NpgsqlOptions"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IDecafOptionsBuilder"/> to use.</param>
        /// <param name="options">The <see cref="NpgsqlOptions"/> to use.</param>
        public static IDecafOptionsBuilder UseNpgsql(
            this IDecafOptionsBuilder builder,
            NpgsqlOptions options)
        {
            var x = builder as IDecafOptionsBuilderExtensions;
            x.UseDbImplementation<NpgsqlImplementationFactory>(options);
            return builder;
        }
    }
}

