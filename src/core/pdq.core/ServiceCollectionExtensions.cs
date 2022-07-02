using System;
using Microsoft.Extensions.DependencyInjection;
using pdq.common.Logging;
using pdq.common;

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
        public static IServiceCollection AddPdq(this IServiceCollection services, Action<PdqOptions> options)
        {
			var o = new PdqOptions();
			options(o);
			return AddPdq(services, o);
        }

        /// <summary>
        /// Add pdq to your <see cref="IServiceCollection"/>, providing an instance
        /// of pre-configured pdq options to be used.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add to.</param>
        /// <param name="options">A set of pre-configured <see cref="PdqOptions"/>.</param>
        /// <returns></returns>
        public static IServiceCollection AddPdq(this IServiceCollection services, PdqOptions options)
        {
			services.AddSingleton(options);
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			return services;
		}
	}
}

