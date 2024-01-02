using Microsoft.Extensions.DependencyInjection;

namespace pdq.db.common
{
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

