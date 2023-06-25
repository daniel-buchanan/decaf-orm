using Microsoft.Extensions.DependencyInjection;
using pdq.common.Options;

namespace pdq.db.common
{
    public static class PdqOptionsBuilderExtensions
	{
		/// <summary>
		/// Configure a given Database Implementation.
		/// </summary>
		/// <typeparam name="T">The type of the database implementation factory to use</typeparam>
		/// <param name="self">The <see cref="IPdqOptionsBuilderExtensions"/> instance to use.</param>
		/// <param name="options">The <see cref="IDatabaseOptions"/> to configure.</param>
		public static void UseDbImplementation<T>(
			this IPdqOptionsBuilderExtensions self,
			IDatabaseOptions options)
			where T : IDbImplementationFactory, new()
		{
			var x = new T();
			var internalOptions = options as IDatabaseOptionsExtensions;
			x.Configure(self.Services, internalOptions);
		}
	}

	public interface IDbImplementationFactory
	{
		/// <summary>
		/// Configure the database implementation and related services.
		/// </summary>
		/// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
		/// <param name="options">The <see cref="IDatabaseOptions"/> to configure.</param>
		void Configure(IServiceCollection services, IDatabaseOptionsExtensions options);
	}
}

