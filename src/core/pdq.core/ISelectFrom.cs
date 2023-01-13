﻿using System;

namespace pdq
{
	public interface ISelectFrom :
		IExecute,
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