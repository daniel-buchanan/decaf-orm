using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace pdq.db.common
{
	public class OptionsBuilder<T>
        where T: new()
	{
        private readonly Dictionary<string, object> values = new Dictionary<string, object>();

        public OptionsBuilder() { }

        /// <inheritdoc/>
        public T Build()
        {
            var options = new T();
            var optionsType = typeof(T);
            var flags = BindingFlags.Instance | BindingFlags.Public;
            var properties = optionsType.GetProperties(flags);

            foreach (var p in values)
            {
                var prop = properties.FirstOrDefault(op => op.Name == p.Key);
                if (prop == null) continue;
                prop.SetValue(options, p.Value);
            }

            return options;
        }

        /// <inheritdoc/>
        protected void ConfigureProperty<TValue>(string property, TValue value)
            => this.values.Add(property, value);
    }
}

