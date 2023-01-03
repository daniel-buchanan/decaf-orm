using System;
using System.Collections.Generic;
using System.Linq;
using pdq.common.Templates;
using pdq.common.Utilities;

namespace pdq.common.Templates
{
    public class ParameterManager : IParameterManager
    {
        public const string ParameterPrefix = "@";
        private const string ParameterName = "p";

        private readonly IHashProvider hashProvider;

        private readonly Dictionary<string, SqlParameter> parameters;
        private readonly Dictionary<string, object> parameterValues;
        private int parameterCount;

        public ParameterManager(IHashProvider hashProvider)
        {
            this.hashProvider = hashProvider;
            this.parameters = new Dictionary<string, SqlParameter>();
            this.parameterValues = new Dictionary<string, object>();
            this.parameterCount = 0;
        }

        public static IParameterManager Create(IHashProvider hashProvider)
            => new ParameterManager(hashProvider);

        public SqlParameter Add<T>(T state, object value)
        {
            var hash = this.hashProvider.GetHash(state, value);
            var existing = this.parameters.TryGetValue(hash, out var existingParameter);
            if (existing) return existingParameter;

            var parameterNumber = parameterCount + 1;

            var parameterName = $"{ParameterPrefix}{ParameterName}{parameterNumber}";
            var newParameter = SqlParameter.Create(hash, parameterName);
            this.parameters.Add(hash, newParameter);
            this.parameterValues.Add(hash, value);

            this.parameterCount++;

            return newParameter;
        }

        public void Clear()
        {
            this.parameters.Clear();
            this.parameterValues.Clear();
            this.parameterCount = 0;
        }

        public IEnumerable<SqlParameter> GetParameters()
            => this.parameters.Select(kp => kp.Value);

        public Dictionary<string, object> GetParameterValues()
        {
            var result = new Dictionary<string, object>();
            foreach(var kp in this.parameters)
            {
                var parameterName = kp.Value.Name.Replace(ParameterPrefix, string.Empty);
                result.Add(parameterName, this.parameterValues[kp.Key]);
            }

            return result;
        }
    }
}

