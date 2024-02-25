using System;
namespace decaf.common.Connections
{
	public enum ConnectionAuthenticationType
	{
		/// <summary>
		/// Username and Password authentication.
		/// </summary>
		UsernamePassword,

		/// <summary>
		/// Integrated authentication, using the account running this process.
		/// </summary>
		Integrated,

		/// <summary>
		/// The authentication method is fetched later when the connection string is created.
		/// </summary>
		DelayedFetch,

		/// <summary>
		/// No authentication.
		/// </summary>
		None
	}

	public interface IConnectionAuthentication
	{
		/// <summary>
		/// The type of authentication.
		/// </summary>
		ConnectionAuthenticationType AuthenticationType { get; }
	}
}

