using System.Collections.Generic;
using System.Linq;
using decaf.common.Utilities;

namespace decaf.common.Templates
{
    public class ParameterManager : IParameterManager
    {
        private const string DefaultPrefix = "@";
        private const string ParameterName = "p";
        private string parameterPrefix;

        private readonly IHashProvider hashProvider;

        private readonly Dictionary<string, SqlParameter> parameters;
        private readonly Dictionary<string, object> parameterValues;
        private int parameterCount;

        public ParameterManager(IHashProvider hashProvider)
        {
            this.hashProvider = hashProvider;
            parameters = new Dictionary<string, SqlParameter>();
            parameterValues = new Dictionary<string, object>();
            parameterCount = 0;
        }

        protected virtual string GetParameterPrefix()
            => DefaultPrefix;

        private string ParameterPrefix
        {
            get
            {
                if (string.IsNullOrEmpty(parameterPrefix))
                    parameterPrefix = GetParameterPrefix();
                return parameterPrefix;
            }
        }
        
        public static IParameterManager Create(IHashProvider hashProvider)
            => new ParameterManager(hashProvider);

        public SqlParameter Add<T>(T state, object value)
        {
            var hash = hashProvider.GetHash(state, value);
            var existing = parameters.TryGetValue(hash, out var existingParameter);
            if (existing) return existingParameter;

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
}

