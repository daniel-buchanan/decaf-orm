using System;
using Microsoft.Extensions.DependencyInjection;
using pdq.common.Options;
using pdq.db.common;
using pdq.db.common.Builders;
using pdq.state;

namespace pdq.sqlserver
{
	public static class PdqOptionsBuilderExtensions
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
            builder.ConfigureDbImplementation<SqlServerSqlFactory, SqlServerConnectionFactory, SqlServerTransactionFactory>();
            builder.Services.AddSingleton(options.ConnectionDetails);
            builder.Services.AddSingleton(options);
            builder.Services.AddSingleton<IValueParser, SqlServerValueParser>();
            builder.Services.AddSingleton<Builders.QuotedIdentifierBuilder>();
            builder.Services.AddTransient<db.common.Builders.IWhereBuilder, Builders.WhereBuilder>();
            builder.Services.AddTransient<IBuilderPipeline<ISelectQueryContext>, Builders.SelectBuilderPipeline>();
            builder.Services.AddTransient<IBuilderPipeline<IDeleteQueryContext>, Builders.DeleteBuilderPipeline>();
            builder.Services.AddTransient<IBuilderPipeline<IInsertQueryContext>, Builders.InsertBuilderPipeline>();
            builder.Services.AddTransient<IBuilderPipeline<IUpdateQueryContext>, Builders.UpdateBuilderPipeline>();
        }
    }
}

