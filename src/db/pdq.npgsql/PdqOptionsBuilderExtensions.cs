using System;
using pdq.common.Options;
using pdq.db.common;

namespace pdq.npgsql
{
    public static partial class PdqOptionsBuilderExtensions
	{
        /// <summary>
        /// Use PostgreSQL (Npgsql as the client) with the default configuration and options.
        /// </summary>
        /// <param name="builder">The <see cref="IPdqOptionsBuilder"/> to use.</param>
        public static void UseNpgsql(this IPdqOptionsBuilder builder)
            => UseNpgsql(builder, new NpgsqlOptions());

        /// <summary>
        /// Use PostgreSQL (Npgsql as the client) with a builder for the Postgres options.
        /// </summary>
        /// <param name="optionsBuilder">The <see cref="IPdqOptionsBuilder"/> to use.</param>
        /// <param name="builder">An <see cref="Action{T}"/> to configure the <see cref="NpgsqlOptions"/>.</param>
        public static void UseNpgsql(
            this IPdqOptionsBuilder optionsBuilder,
            Action<INpgsqlOptionsBuilder> builder)
        {
            var options = new NpgsqlOptionsBuilder();
            builder(options);
            UseNpgsql(optionsBuilder, options.Build());
        }

        /// <summary>
        /// Use PostgreSQL (Npgsql as the client) with a provided <see cref="NpgsqlOptions"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IPdqOptionsBuilder"/> to use.</param>
        /// <param name="options">The <see cref="NpgsqlOptions"/> to use.</param>
        public static void UseNpgsql(
            this IPdqOptionsBuilder builder,
            NpgsqlOptions options)
        {
            var x = builder as IPdqOptionsBuilderExtensions;
            x.UseDbImplementation<NpgsqlImplementationFactory>(options);
        }
    }
}

