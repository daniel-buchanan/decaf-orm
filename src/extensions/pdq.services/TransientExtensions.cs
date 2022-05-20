using System;
using pdq.core;

namespace pdq.services
{
	public static class TransientExtensions
	{
		public static IService<T> GetService<T>(this ITransient transient)
        {
			
        }

		public static IQuery<T> GetQueryService<T>(this ITransient transient)
        {

        }

		public static ICommand<T> GetCommandService<T>(this ITransient transient)
        {

        }
	}
}

