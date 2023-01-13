using System;
using Microsoft.Extensions.DependencyInjection;
using pdq.common;
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
        /// <param name="optionsBuilder"></param>
        /// <param name="options"></param>
        public static void UseNpgsql(
            this IPdqOptionsBuilder optionsBuilder,
            NpgsqlOptions options)
        {
            var builder = optionsBuilder as IPdqOptionsBuilderInternal;
            builder.SetConnectionFactory<NpgsqlConnectionFactory>();
            builder.SetTransactionFactory<NpgsqlTransactionFactory>();
            builder.SetSqlFactory<NpgsqlSqlFactory>();
            builder.Services.AddSingleton(options);
            builder.Services.AddSingleton<IValueParser, NpgsqlValueParser>();
            builder.Services.AddSingleton<Builders.QuotedIdentifierBuilder>();
            builder.Services.AddTransient<db.common.Builders.IWhereBuilder, Builders.WhereBuilder>();
            builder.Services.AddTransient<IBuilder<ISelectQueryContext>, Builders.SelectBuilder>();
            builder.Services.AddTransient<IBuilder<IDeleteQueryContext>, Builders.DeleteBuilder>();
        }
    }
}

