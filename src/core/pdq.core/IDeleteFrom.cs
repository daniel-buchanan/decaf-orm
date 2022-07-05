using System;
using System.Linq.Expressions;

namespace pdq
{
	public interface IDeleteFrom : IGetSql, IExecute
	{
		/// <summary>
        /// Restrict the rows that are deleted by this query.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
		IDeleteFrom Where(Action<IWhereBuilder> builder);
	}

	public interface IDeleteFrom<T> : IGetSql, IExecute
    {
		/// <summary>
        /// 
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
		IDeleteFrom<T> Where(Expression<Func<T, bool>> whereExpression);
    }
}

