using System;

namespace pdq
{
	public interface ISelectFrom :
		ISelectColumn,
		IJoin
	{
		/// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
		IGroupBy Where(Action<IWhereBuilder> builder);
	}
}