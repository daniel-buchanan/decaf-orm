using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace pdq.common.Options
{
	public class OptionsBuilder<T> : IOptionsBuilder<T>
		where T: class, new()
	{
        private readonly Dictionary<string, object> values = new Dictionary<string, object>();

        public OptionsBuilder()
		{
		}

        /// <inheritdoc/>
        public T Build()
        {
            var options = new T();
            var optionsType = typeof(T);
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var properties = optionsType.GetProperties(flags);

            foreach (var p in values)
            {
                var prop = properties.FirstOrDefault(op => op.Name == p.Key);
                if (prop == null) continue;
                prop.SetValue(options, p.Value);
            }

            return options;
        }

        /// <summary>
        /// Configure a property to be set when the options are built.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to add.</typeparam>
        /// <param name="property">The property to configure.</param>
        /// <param name="value">The value to set the property to.</param>
        protected void ConfigureProperty<TValue>(string property, TValue value)
            => this.values.Add(property, value);
    }
}

