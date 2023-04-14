using System;
using System.Data;

namespace pdq.common.Connections
{
	public interface IConnection : IDisposable
	{
		/// <summary>
        /// Open the connection.
        /// </summary>
		void Open();

		/// <summary>
        /// Close the connection.
        /// </summary>
		void Close();

		/// <summary>
        /// Get the underlying database connection.
        /// </summary>
        /// <returns>An <see cref="IDbConnection"/> for the underlying connection.</returns>
		IDbConnection GetUnderlyingConnection();

		/// <summary>
        /// Get the underlying database connection, converting it to a specific type.<br/>
        /// The type provided MUST inherit from <see cref="IDbConnection"/>.
        /// </summary>
        /// <typeparam name="TConnection">The type to return the connection as.</typeparam>
        /// <returns>The underlying connection, cast as specified.</returns>
		TConnection GetUnderlyingConnectionAs<TConnection>() where TConnection: IDbConnection;

		/// <summary>
        /// Get the connection state.
        /// </summary>
		ConnectionState State { get; }

		/// <summary>
		/// Get the hash for this connection.
		/// </summary>
		/// <returns>The hash as a Base 64 encoded string.</returns>
		string GetHash();
	}
}

