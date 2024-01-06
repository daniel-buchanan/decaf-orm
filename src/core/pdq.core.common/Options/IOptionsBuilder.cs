using System;
using System.Linq.Expressions;

namespace pdq.common.Options
{
	public interface IOptionsBuilder<T>
		where T: class, new()
	{
        /// <summary>
        /// Build the Options instance.
        /// </summary>
        /// <returns>A built Options instance</returns>
        T Build();

        void ConfigureProperty<TValue>(Expression<Func<T, TValue>> expr, TValue value);
	}
}

