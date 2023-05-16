using System;
using Microsoft.Extensions.DependencyInjection;
using pdq.common.Options;
using pdq.db.common;
using pdq.db.common.Builders;
using pdq.state;

namespace pdq.npgsql
{
	public static class PdqOptionsBuilderExtensions
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="optionsBuilder"></param>
        public static void UseNpgsql(this IPdqOptionsBuilder optionsBuilder)
            => UseNpgsql(optionsBuilder, new NpgsqlOptions());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="builder"></param>
        public static void UseNpgsql(
            this IPdqOptionsBuilder optionsBuilder,
            Action<INpgsqlOptionsBuilder> builder)
        {
            var options = new NpgsqlOptionsBuilder();
            builder(options);
            UseNpgsql(optionsBuilder, options.Build());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="options"></param>
        public static void UseNpgsql(
            this IPdqOptionsBuilder builder,
            NpgsqlOptions options)
        {
            builder.ConfigureDbImplementation<NpgsqlSqlFactory, NpgsqlConnectionFactory, NpgsqlTransactionFactory>();
            builder.Services.AddSingleton(options.ConnectionDetails);
            builder.Services.AddSingleton(options);
            builder.Services.AddSingleton<IDatabaseOptions>(options);
            builder.Services.AddSingleton<IValueParser, NpgsqlValueParser>();
            builder.Services.AddSingleton<db.common.Builders.IConstants, db.common.ANSISQL.Constants>();
            builder.Services.AddSingleton<db.common.Builders.IQuotedIdentifierBuilder, db.common.ANSISQL.QuotedIdentifierBuilder>();
            builder.Services.AddTransient<db.common.Builders.IWhereBuilder, Builders.WhereBuilder>();
            builder.Services.AddTransient<IBuilderPipeline<ISelectQueryContext>, Builders.SelectBuilderPipeline>();
            builder.Services.AddTransient<IBuilderPipeline<IDeleteQueryContext>, Builders.DeleteBuilderPipeline>();
            builder.Services.AddTransient<IBuilderPipeline<IInsertQueryContext>, Builders.InsertBuilderPipeline>();
            builder.Services.AddTransient<IBuilderPipeline<IUpdateQueryContext>, Builders.UpdateBuilderPipeline>();
        }
    }
}

