using System;
using pdq.common;
using pdq.common.Connections;
using pdq.common.Options;

namespace pdq.db.common
{
	public interface ISqlOptionsBuilder<out TOptions, TSelf> :
        IOptionsBuilder<TOptions>
		where TOptions: class, IDatabaseOptions, new()
	{
        /// <summary>
        /// Set the connection details.
        /// </summary>
        /// <param name="connectionDetails">The connection details to use.</param>
        /// <returns>(Fluent API) The ability to perform other setup actions.</returns>
        TSelf WithConnectionDetails(IConnectionDetails connectionDetails);
	}
}

