using System;
using System.Threading;
using System.Threading.Tasks;

namespace decaf.common.Connections
{
	public abstract class ConnectionAuthentication : IConnectionAuthentication
	{
        /// <inheritdoc/>
        public ConnectionAuthenticationType AuthenticationType { get; protected set; }
    }

    public class UsernamePasswordAuthentication : ConnectionAuthentication
    {
        public UsernamePasswordAuthentication() => AuthenticationType = ConnectionAuthenticationType.UsernamePassword;

        public UsernamePasswordAuthentication(string username, string password) : this()
        {
            Username = username;
            Password = password;
        }

        /// <summary>
        /// The username to use for the connection.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The password for the user.
        /// </summary>
        public string Password { get; set; }
    }

    public class IntegratedAuthentication : ConnectionAuthentication
    {
        public IntegratedAuthentication() => AuthenticationType = ConnectionAuthenticationType.Integrated;
    }

    public class DelayedFetchAuthentication : ConnectionAuthentication
    {
        public DelayedFetchAuthentication(Func<CancellationToken, Task<IConnectionAuthentication>> fetchDelegate)
        {
            AuthenticationType = ConnectionAuthenticationType.DelayedFetch;
            FetchAsync = fetchDelegate;
        }

        /// <summary>
        /// Delegate method for fetching credentials asynchronously.
        /// </summary>
        public Func<CancellationToken, Task<IConnectionAuthentication>> FetchAsync { get; }
    }

    public class NoAuthentication : ConnectionAuthentication
    {
        public NoAuthentication() => AuthenticationType = ConnectionAuthenticationType.None;
    }
}

