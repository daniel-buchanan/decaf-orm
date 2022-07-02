using System;
using System.Linq.Expressions;

namespace pdq
{
	public interface IDeleteFrom : IBuilder, IExecute
	{
		/// <summary>
        /// Restrict the rows that are deleted by this query.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
		IDeleteFrom Where(Action<IWhereBuilder> builder);
	}

	public interface IDeleteFrom<T> : IBuilder, IExecute
    {
		/// <summary>
        /// 
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
		IDeleteFrom<T> Where(Expression<Func<T, bool>> whereExpression);
    }
}

