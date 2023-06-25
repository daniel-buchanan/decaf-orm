using System;
using pdq.common.Options;
using pdq.db.common;

namespace pdq.sqlserver
{
    public static partial class PdqOptionsBuilderExtensions
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="optionsBuilder"></param>
        public static void UseNpgsql(this IPdqOptionsBuilder optionsBuilder)
            => UseSqlServer(optionsBuilder, new SqlServerOptions());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="builder"></param>
        public static void UseSqlServer(
            this IPdqOptionsBuilder optionsBuilder,
            Action<ISqlServerOptionsBuilder> builder)
        {
            var options = new SqlServerOptionsBuilder();
            builder(options);
            UseSqlServer(optionsBuilder, options.Build());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="options"></param>
        public static void UseSqlServer(
            this IPdqOptionsBuilder builder,
            SqlServerOptions options)
        {
            var x = builder as IPdqOptionsBuilderExtensions;
            x.UseDbImplementation<SqlServerImplementationFactory>(options);
        }
    }
}

