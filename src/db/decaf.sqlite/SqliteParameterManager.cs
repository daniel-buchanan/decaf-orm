using decaf.common.Templates;
using decaf.common.Utilities;

namespace decaf.sqlite;

public class SqliteParameterManager : ParameterManager
{
    private const string Prefix = ":";
    
    public SqliteParameterManager(IHashProvider hashProvider) : base(hashProvider) { }

    protected override string GetParameterPrefix()
        => Prefix;
}