using System.Collections.Generic;
using System.Linq;
using decaf.common.Utilities;

namespace decaf.common.Templates;

public class ParameterManager(IHashProvider hashProvider) : IParameterManager
{
    private const string DefaultPrefix = "@";
    private const string ParameterName = "p";
    private string? parameterPrefix;

    private readonly Dictionary<string, SqlParameter> parameters = new();
    private readonly Dictionary<string, object> parameterValues = new();
    private int parameterCount;

    protected virtual string GetParameterPrefix()
        => DefaultPrefix;

    private string ParameterPrefix
    {
        get
        {
            if (parameterPrefix.IsNullOrWhiteSpace())
                parameterPrefix = GetParameterPrefix();
            return parameterPrefix!;
        }
    }
        
    public static IParameterManager Create(IHashProvider hashProvider)
        => new ParameterManager(hashProvider);

    public SqlParameter Add<T>(T state, object value)
    {
        var hash = hashProvider.GetHash(state, value);
        if (parameters.TryGetValue(hash, out SqlParameter existingParameter))
            return existingParameter;

        var parameterNumber = parameterCount + 1;

        var parameterName = $"{ParameterPrefix}{ParameterName}{parameterNumber}";
        var newParameter = SqlParameter.Create(hash, parameterName);
        parameters.Add(hash, newParameter);
        parameterValues.Add(hash, value);

        parameterCount++;

        return newParameter;
    }

    public void Clear()
    {
        parameters.Clear();
        parameterValues.Clear();
        parameterCount = 0;
    }

    public IEnumerable<SqlParameter> GetParameters()
        => parameters.Select(kp => kp.Value);

    public Dictionary<string, object> GetParameterValues(bool includePrefix = false)
    {
        var result = new Dictionary<string, object>();
        foreach(var kp in parameters)
        {
            var parameterName = kp.Value.Name;
            if (!includePrefix) 
                parameterName = parameterName.Replace(ParameterPrefix, string.Empty);
                
            result.Add(parameterName, parameterValues[kp.Key]);
        }

        return result;
    }
}