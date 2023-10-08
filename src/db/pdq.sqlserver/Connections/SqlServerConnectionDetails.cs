using System.Text;
using System.Threading;
using System.Threading.Tasks;
using pdq.common.Connections;
using pdq.common.Exceptions;

namespace pdq.sqlserver
{
    public class SqlServerConnectionDetails :
        ConnectionDetails,
        ISqlServerConnectionDetails
    {
        private const string MarsEnabled = "MultipleActiveResultSets=True";
        private const string TrustedConnection = "Trusted_Connection=Yes";
        private const string UsernameRegex = @"User ID=(.+);";
        private const string PasswordRegex = @"Password=(.+);";
        private const string UserID = "User ID";

        private bool isTrustedConnection;
        private bool isMarsEnabled;

        public SqlServerConnectionDetails() : base()
            => this.isTrustedConnection = false;

        private SqlServerConnectionDetails(string connectionString)
            : base(connectionString)
            => ParseConnectionString(connectionString, (c) =>
            {
                if (c.Contains(MarsEnabled))
                    this.isMarsEnabled = true;

                this.isTrustedConnection = c.Contains(TrustedConnection);

                if(c.Contains(UserID))
                {
                    var username = MatchAndFetch(UsernameRegex, c, s => s);
                    var password = MatchAndFetch(PasswordRegex, c, s => s);
                    Authentication = new UsernamePasswordAuthentication(username, password);
                }
            });

        /// <inheritdoc/>
        protected override string HostRegex => @"Server=(.+),(\d+);";

        /// <inheritdoc/>
        protected override string PortRegex => @"Server=.+,(\d+);";

        /// <inheritdoc/>
        protected override string DatabaseRegex => @"Database=(.+);";

        /// <summary>
        /// Create a new <see cref="ISqlServerConnectionDetails"/> from a provided connection string.
        /// </summary>
        /// <param name="connectionString">The connection string to use.</param>
        /// <returns>A new <see cref="ISqlServerConnectionDetails"/> object.</returns>
        public static ISqlServerConnectionDetails FromConnectionString(string connectionString)
            => new SqlServerConnectionDetails(connectionString);

        /// <summary>
        /// Create an <see cref="ISqlServerConnectionDetails"/> from a provided connection string, using seperately provided credentials
        /// </summary>
        /// <param name="connectionString">The connection string to use.</param>
        /// <param name="authentication">The <see cref="IConnectionAuthentication"/> to use.</param>
        /// <returns>A new <see cref="ISqlServerConnectionDetails"/> object.</returns>
        public static ISqlServerConnectionDetails FromConnectionString(
            string connectionString,
            IConnectionAuthentication authentication)
        {
            var details = new SqlServerConnectionDetails(connectionString);
            details.Authentication = authentication;
            return details;
        }

        /// <inheritdoc/>
        protected override int DefaultPort
            => 1433;

        /// <inheritdoc/>
        public void EnableMars()
            => this.isMarsEnabled = true;

        /// <inheritdoc/>
        public void IsTrustedConnection()
            => this.isTrustedConnection = true;

        /// <inheritdoc/>
        protected override async Task<string> ConstructConnectionStringAsync(CancellationToken cancellationToken = default)
        {
            string username, password;
            username = password = null;
            var auth = Authentication;

            // check for delayed fetch
            if(auth is DelayedFetchAuthentication delayedFetch)
                auth = await delayedFetch.FetchAsync(cancellationToken);

            if(auth is UsernamePasswordAuthentication creds)
            {
                username = creds.Username;
                password = creds.Password;
            }

            var sb = new StringBuilder();
            sb.AppendFormat("Server={0},{1};", this.Hostname, this.Port);
            sb.AppendFormat("Database={0};", this.DatabaseName);

            if (this.isTrustedConnection)
                sb.AppendFormat("{0};", TrustedConnection);
            else
            {
                sb.AppendFormat("User ID={0};", username);
                sb.AppendFormat("Password={0};", password);
            }

            if (this.isMarsEnabled)
                sb.AppendFormat("{0};", MarsEnabled);
            
            return sb.ToString();
        }

        protected override ConnectionStringParsingException ValidateConnectionString(string connectionString)
        {
            if (connectionString?.Contains("Server") == false)
                return new ConnectionStringParsingException("Connection String does not contain a \"Server\" parameter.");

            if (connectionString?.Contains("Database") == false)
                return new ConnectionStringParsingException("Connection String does not contain a \"Database\" parameter.");

            if (connectionString?.Contains(UserID) == true && connectionString?.Contains("Password") == false)
                return new ConnectionStringParsingException("Connection String User credentials are missing a \"Password\".");

            if (connectionString?.Contains(UserID) == false && connectionString?.Contains("Password") == true)
                return new ConnectionStringParsingException("Connection String User credentials are missing a \"User ID\".");

            if (connectionString?.Contains(UserID) == false &&
                connectionString?.Contains("Password") == false &&
                connectionString?.Contains(TrustedConnection) == false)
                return new ConnectionStringParsingException("Connection String does not have eitehr \"User ID\" and \"Password\" or \"Trusted Credentials\" set.");


            return null;
        }
    }
}

