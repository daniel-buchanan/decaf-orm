using System;
using System.Threading.Tasks;

namespace pdq.core.common
{
	public interface IConnectionDetails : IDisposable, IAsyncDisposable
	{
		/// <summary>
        /// The hostname of the database server.
        /// </summary>
		string Hostname { get; }

		/// <summary>
        /// The port the database runs on.
        /// </summary>
		int Port { get; }

		/// <summary>
        /// The name  of the database.
        /// </summary>
		string DatabaseName { get; }

		/// <summary>
        /// Gets the connection string to use to connect to the database.
        /// </summary>
        /// <returns>The connection string.</returns>
		string GetConnectionString();

		/// <summary>
        /// Gets the connection string to connect to the database.
        /// </summary>
        /// <returns>A task which returns the connection string.</returns>
		Task<string> GetConnectionStringAsync();

		/// <summary>
        /// Gets the unique hash for this connection.
        /// </summary>
        /// <returns>The unique hash for the connection details.</returns>
		internal string GetHash();
	}
}

