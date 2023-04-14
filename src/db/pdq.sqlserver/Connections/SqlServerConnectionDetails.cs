using System.Text;
using System.Threading.Tasks;
using pdq.common.Connections;
using pdq.db.common.Exceptions;

namespace pdq.sqlserver
{
    public class SqlServerConnectionDetails :
        ConnectionDetails,
        ISqlServerConnectionDetails
    {
        private bool isTrustedConnection;

        public SqlServerConnectionDetails() : base()
        {
            this.isTrustedConnection = false;
        }

        /// <inheritdoc/>
        protected override int DefaultPort
            => 1433;

        /// <inheritdoc/>
        public void IsTrustedConnection()
            => this.isTrustedConnection = true;

        /// <inheritdoc/>
        protected override async Task<string> ConstructConnectionStringAsync()
        {
            string username, password;
            var auth = Authentication;

            // check for delayed fetch
            if(auth is DelayedFetchAuthentication delayedFetch)
                auth = await delayedFetch.FetchAsync();

            if(auth is UsernamePasswordAuthentication creds)
            {
                username = creds.Username;
                password = creds.Password;
            }
            else
            {
                throw new ConnectionStringConstructException("Provided Credentials are not username an password.");
            }

            var sb = new StringBuilder();
            sb.AppendFormat("Host={0};", this.Hostname);
            sb.AppendFormat("Port={0};", this.Port);
            sb.AppendFormat("Database={0};", this.DatabaseName);
            sb.AppendFormat("Username={0};", username);
            sb.AppendFormat("Password={0};", password);

            if (this.isTrustedConnection)
                sb.Append("Trusted Connection=True;");
            
            return sb.ToString();
        }
    }
}

