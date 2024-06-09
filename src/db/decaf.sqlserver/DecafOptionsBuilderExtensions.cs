using System;
using decaf.common.Options;
using decaf.db.common;

namespace decaf.sqlserver
{
    public static partial class DecafOptionsBuilderExtensions
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="optionsBuilder"></param>
        public static void UseSqlServer(this IDecafOptionsBuilder optionsBuilder)
            => UseSqlServer(optionsBuilder, new SqlServerOptions());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="builder"></param>
        public static IDecafOptionsBuilder UseSqlServer(
            this IDecafOptionsBuilder optionsBuilder,
            Action<ISqlServerOptionsBuilder> builder)
        {
            var options = new SqlServerOptionsBuilder();
            builder(options);
            UseSqlServer(optionsBuilder, options.Build());
            return optionsBuilder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="options"></param>
        public static void UseSqlServer(
            this IDecafOptionsBuilder builder,
            SqlServerOptions options)
        {
            var x = builder as IDecafOptionsBuilderExtensions;
            x.UseDbImplementation<SqlServerImplementationFactory>(options);
        }
    }
}

