using System.Text;
using System.Text.RegularExpressions;
using decaf.common.Connections;
using decaf.common.Exceptions;

namespace decaf.sqlite;

public class SqliteConnectionDetails : 
    ConnectionDetails, 
    ISqliteConnectionDetails
{
    private bool? createNew;
    private bool? inMemory;
    private string? fullUri;
    private bool? readOnly;
    
    public SqliteConnectionDetails() { }

    public SqliteConnectionDetails(string connectionString) : base(connectionString) 
        => ParseConnectionString(connectionString);

    public SqliteConnectionDetails(SqliteOptions options)
    {
        FullUri = options.DatabasePath;
        InMemory = options.InMemory;
        ReadOnly = options.ReadOnly;
    }

    public static ISqliteConnectionDetails FromConnectionString(string connectionString)
        => new SqliteConnectionDetails(connectionString);
    
    /// <summary>
    /// Not Implemented as only called from base, and calling method is overridden.
    /// </summary>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    protected override ConnectionStringParsingException ValidateConnectionString(string connectionString) 
        => throw new NotImplementedException();

    protected override string HostRegex => throw new NotImplementedException();
    protected override string PortRegex => throw new NotImplementedException();
    protected override string DatabaseRegex => throw new NotImplementedException();
    protected override int DefaultPort => throw new NotImplementedException();

    public string? FullUri
    {
        get => fullUri ?? string.Empty;
        set
        {
            if(!string.IsNullOrWhiteSpace(fullUri))
            {
                throw new ConnectionModificationException($"{nameof(FullUri)} cannot be modified once ConnectionDetails instance created");
            }

            fullUri = value;
        }
    }

    public bool InMemory
    {
        get => inMemory ?? false;
        set
        {
            if(inMemory != null)
            {
                throw new ConnectionModificationException($"{nameof(InMemory)} cannot be modified once ConnectionDetails instance created");
            }

            inMemory = value;
        }
    }

    public bool CreateNew
    {
        get => createNew ?? false;
        set
        {
            if(createNew != null)
            {
                throw new ConnectionModificationException($"{nameof(CreateNew)} cannot be modified once ConnectionDetails instance created");
            }

            createNew = value;
        }
    }

    public bool ReadOnly
    {
        get => readOnly ?? false;
        set
        {
            if(readOnly != null)
            {
                throw new ConnectionModificationException($"{nameof(ReadOnly)} cannot be modified once ConnectionDetails instance created");
            }

            readOnly = value;
        }
    }

    protected sealed override void ParseConnectionString(string input, Action<string>? additionalParsing = null)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ConnectionStringParsingException("Cannot parse NULL connection string.");
        
        var regex = new Regex(@"Data Source=([a-zA-Z0-9\\_\-\.\:\/]+);(?:Mode=((?:[Rr]eadOnly){0,1}(?:[Rr]ead[Ww]rite){0,1}(?:[Rr]ead[Ww]rite[Cc]reate){0,1});){0,1}", RegexOptions.Compiled | RegexOptions.Singleline, TimeSpan.FromMilliseconds(100));
        var match = regex.Match(input);
        if (!match.Success)
            throw new ConnectionStringParsingException("Cannot parse Connection String: " + input);

        FullUri = match.Groups[1].Value;

        var mode = match.Groups[2].Value;
        switch (mode.ToLower())
        {
            case "readonly":
                ReadOnly = true;
                break;
            case "readwritecreate":
                ReadOnly = false;
                CreateNew = true;
                break;
            default:
                ReadOnly = false;
                break;
        }
        
        if (match.Groups[3].Value == "New=True;")
            CreateNew = true;

        if (FullUri == ":memory:")
            InMemory = true;
    }

    protected override Task<string> ConstructConnectionStringAsync(CancellationToken cancellationToken = default)
    {
        if (InMemory)
        {
            FullUri = ":memory:";
            CreateNew = true;
        }

        var sb = new StringBuilder();
        sb.AppendFormat("Data Source={0};", FullUri);
        if (ReadOnly && !CreateNew) sb.Append("Mode=ReadOnly;");
        if (CreateNew && !ReadOnly) sb.Append("Mode=ReadWriteCreate;");
        if (!CreateNew && !ReadOnly) sb.Append("Mode=ReadWrite;");

        var connectionString = sb.ToString();
        return Task.FromResult(connectionString);
    }
}