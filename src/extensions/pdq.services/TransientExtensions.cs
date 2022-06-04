using System;
using pdq;
using pdq.common;

namespace pdq.services
{
	public static class TransientExtensions
	{
		public static IService<T> GetService<T>(this ITransient transient)
			where T: IEntity
        {
			throw new NotImplementedException();
        }

		public static IQuery<T> GetQuery<T>(this ITransient transient)
			where T: IEntity
        {
			throw new NotImplementedException();
		}

		public static ICommand<T> GetCommand<T>(this ITransient transient)
			where T: IEntity
        {
			throw new NotImplementedException();
		}
	}
}

