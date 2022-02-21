using System.Threading.Tasks;

namespace pdq.core
{
	public interface IConnection
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
		Task<string> GetConnectionString();
	}
}

