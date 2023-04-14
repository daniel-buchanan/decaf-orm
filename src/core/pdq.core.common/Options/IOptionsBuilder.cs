using System;
namespace pdq.common.Options
{
	public interface IOptionsBuilder<out T>
		where T: class, new()
	{
        /// <summary>
        /// Build the Options instance.
        /// </summary>
        /// <returns>A built Options instance</returns>
        T Build();
	}
}

