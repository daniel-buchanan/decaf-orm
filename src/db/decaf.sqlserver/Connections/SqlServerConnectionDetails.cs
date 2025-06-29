﻿using System.Text;
using System.Threading;
using System.Threading.Tasks;
using decaf.common.Connections;
using decaf.common.Exceptions;

namespace decaf.sqlserver;

public class SqlServerConnectionDetails :
    ConnectionDetails,
    ISqlServerConnectionDetails
{
    private const string MarsEnabled = "MultipleActiveResultSets=True";
    private const string TrustedConnection = "Trusted_Connection=Yes";
    private const string UsernameRegex = @"User ID=(.+);";
    private const string PasswordRegex = @"Password=(.+);";
    private const string UserId = "User ID";

    private bool isTrustedConnection;
    private bool isMarsEnabled;

    public SqlServerConnectionDetails()
        => isTrustedConnection = false;

    private SqlServerConnectionDetails(string connectionString) : base(connectionString) { }

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
        => isMarsEnabled = true;

    /// <inheritdoc/>
    public void IsTrustedConnection()
        => isTrustedConnection = true;

    /// <inheritdoc/>
    protected override async Task<string> ConstructConnectionStringAsync(CancellationToken cancellationToken = default)
    {
        string password;
        var username = password = null;
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
        sb.AppendFormat("Server={0},{1};", Hostname, Port);
        sb.AppendFormat("Database={0};", DatabaseName);

        if (isTrustedConnection)
            sb.AppendFormat("{0};", TrustedConnection);
        else
        {
            sb.AppendFormat("User ID={0};", username);
            sb.AppendFormat("Password={0};", password);
        }

        if (isMarsEnabled)
            sb.AppendFormat("{0};", MarsEnabled);
            
        return sb.ToString();
    }
    
    /// <inheritdoc />
    protected override ConnectionStringParsingException ValidateConnectionString(string connectionString)
    {
        if (connectionString?.Contains("Server") == false)
            return new ConnectionStringParsingException("Connection String does not contain a \"Server\" parameter.");

        if (connectionString?.Contains("Database") == false)
            return new ConnectionStringParsingException("Connection String does not contain a \"Database\" parameter.");

        if (connectionString?.Contains(UserId) == true && !connectionString.Contains("Password"))
            return new ConnectionStringParsingException("Connection String User credentials are missing a \"Password\".");

        if (connectionString?.Contains(UserId) == false && connectionString.Contains("Password"))
            return new ConnectionStringParsingException("Connection String User credentials are missing a \"User ID\".");

        if (connectionString?.Contains(UserId) == false &&
            !connectionString.Contains("Password") &&
            !connectionString.Contains(TrustedConnection))
            return new ConnectionStringParsingException("Connection String does not have either \"User ID\" and \"Password\" or \"Trusted Credentials\" set.");


        return null;
    }

    /// <inheritdoc />
    protected override void ParseConnectionStringInternal(string input)
    {
        if (input.Contains(MarsEnabled))
            isMarsEnabled = true;

        isTrustedConnection = input.Contains(TrustedConnection);

        if (!input.Contains(UserId)) return;
                
        var username = MatchAndFetch(UsernameRegex, input, s => s);
        var password = MatchAndFetch(PasswordRegex, input, s => s);
        Authentication = new UsernamePasswordAuthentication(username, password);
    }
}