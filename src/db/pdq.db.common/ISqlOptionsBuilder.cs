using System;
using Microsoft.Extensions.Configuration;
using pdq.common.Connections;
using pdq.common.Options;

namespace pdq.db.common
{
    public interface ISqlOptionsBuilder<out TOptions, TSelf, TConnectionDetails> :
        IOptionsBuilder<TOptions>
		where TOptions: class, IDatabaseOptionsExtensions, new()
        where TConnectionDetails: IConnectionDetails
	{
        /// <summary>
        /// Set the connection details.
        /// </summary>
        /// <param name="connectionDetails">The connection details to use.</param>
        /// <returns>(Fluent API) The ability to perform other setup actions.</returns>
        TSelf WithConnectionDetails(TConnectionDetails connectionDetails);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configurer"></param>
        /// <returns></returns>
        TSelf WithConnectionDetails(Func<IConfiguration, TConnectionDetails> configurer);

        /// <summary>
        /// Set the connection details, using the pre-configured dependency injection pipeline.
        /// </summary>
        /// <param name="factory">The factory method to create the <see cref="IConnectionDetails"/> to use.</param>
        /// <returns>(FluentAPI) The ability to perfomr other setup actions.</returns>
        TSelf WithConnectionDetails(Func<IServiceProvider, TConnectionDetails> factory);

        /// <summary>
        /// Set the connection details, using a provided connection string.
        /// </summary>
        /// <param name="connectionString">The connection string to use.</param>
        /// <returns>(FluentAPI) The ability to perform other setup actions.</returns>
        TSelf WithConnectionString(string connectionString);
    }
}

