using decaf.common;
using decaf.common.Options;
using decaf.common.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace decaf.db.common;

public static class DecafOptionsBuilderExtensions
{
	/// <summary>
	/// Configure a given Database Implementation.
	/// </summary>
	/// <typeparam name="T">The type of the database implementation factory to use</typeparam>
	/// <param name="self">The <see cref="IDecafOptionsBuilderExtensions"/> instance to use.</param>
	/// <param name="options">The <see cref="IDatabaseOptions"/> to configure.</param>
	public static void UseDbImplementation<T>(
		this IDecafOptionsBuilderExtensions self,
		IDatabaseOptions options)
		where T : IDbImplementationFactory, new()
		=> self.Services.ConfigureDbImplementation<T>(options);

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="services"></param>
	/// <param name="options"></param>
	/// <returns></returns>
	public static IDecafOrmServiceCollection UseDbImplementation<T>(
		this IDecafOrmServiceCollection services,
		IDatabaseOptions options)
		where T : IDbImplementationFactory, new()
	{
		services.ConfigureDbImplementation<T>(options);
		return services;
	}

	private static void ConfigureDbImplementation<T>(
		this IServiceCollection services,
		IDatabaseOptions options)
		where T : IDbImplementationFactory, new()
	{
		var x = new T();
		var internalOptions = options.CastAs<IDatabaseOptionsExtensions>();
		x.Configure(services, internalOptions);
	}
}