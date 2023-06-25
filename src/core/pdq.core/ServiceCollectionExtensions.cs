﻿using System;
using Microsoft.Extensions.DependencyInjection;
using pdq.common.Logging;
using pdq.common;
using pdq.state.Utilities;
using pdq.common.Connections;
using pdq.common.Exceptions;
using pdq.common.Utilities;
using pdq.common.Options;
using pdq.common.Utilities.Reflection;

namespace pdq
{
	public static class ServiceCollectionExtensions
	{
        /// <summary>
        /// Add pdq to your <see cref="IServiceCollection"/>, setting the default options.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add to.</param>
        /// <returns></returns>
        public static IServiceCollection AddPdq(this IServiceCollection services)
        {
			var o = new PdqOptions();
			return AddPdq(services, o);
        }

        /// <summary>
        /// Add pdq to your <see cref="IServiceCollection"/>, providing an action
        /// which will resolve the options.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add to.</param>
        /// <param name="options">An <see cref="Action{T}"/> that resolves the pdq options.</param>
        /// <returns></returns>
        public static IServiceCollection AddPdq(
            this IServiceCollection services,
            Action<IPdqOptionsBuilder> options)
        {
			var b = new PdqOptionsBuilder(services);
			options(b);
			return AddPdq(services, b.Build());
        }

        /// <summary>
        /// Add pdq to your <see cref="IServiceCollection"/>, providing an instance
        /// of pre-configured pdq options to be used.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add to.</param>
        /// <param name="options">A set of pre-configured <see cref="PdqOptions"/>.</param>
        /// <returns></returns>
        public static IServiceCollection AddPdq(
            this IServiceCollection services,
            PdqOptions options)
        {
            ValidateOptions(options);

			services.AddSingleton(options);
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped(typeof(ILoggerProxy), options.LoggerProxyType);
            services.AddSingleton<IHashProvider, HashProvider>();
            services.AddScoped<ITransientFactory, TransientFactory>();
            services.AddSingleton<IReflectionHelper, ReflectionHelper>();
			return services;
		}

        private static void ValidateOptions(PdqOptions options)
        {
            if (options == null)
            {
                throw new PdqOptionsInvalidException($"The provided {nameof(options)} is NULL.");
            }

            if(options.TransactionFactoryType == null ||
               options.ConnectionFactoryType == null)
            {
                throw new PdqOptionsInvalidException($"An implementation must be provided for {nameof(ITransactionFactory)} and {nameof(IConnectionFactory)}");
            }
        }
	}
}

