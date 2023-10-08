using System;
using System.Threading;
using System.Threading.Tasks;

namespace pdq.common.Connections
{
	public interface IConnectionDetails : IDisposable
	{
		/// <summary>
        /// The hostname of the database server.
        /// </summary>
		string Hostname { get; set; }

		/// <summary>
        /// The port the database runs on.
        /// </summary>
		int Port { get; set; }

		/// <summary>
        /// The name  of the database.
        /// </summary>
		string DatabaseName { get; set; }

        /// <summary>
        /// The authentication for the connection.
        /// </summary>
        IConnectionAuthentication Authentication { get; set; }

		/// <summary>
        /// Gets the connection string to use to connect to the database.
        /// </summary>
        /// <returns>The connection string.</returns>
		string GetConnectionString();

		/// <summary>
        /// Gets the connection string to connect to the database.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>A task which returns the connection string.</returns>
		Task<string> GetConnectionStringAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the unique hash for this connection.
        /// </summary>
        /// <returns>The unique hash for the connection details.</returns>
        string GetHash();
    }
}

