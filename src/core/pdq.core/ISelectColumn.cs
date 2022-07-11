using System;
using System.Linq.Expressions;

namespace pdq
{
	public interface ISelectColumn
	{
		IExecuteDynamic Select(Expression<Func<ISelectColumnBuilder, dynamic>> expression);
		IExecute<TResult> Select<TResult>(Expression<Func<ISelectColumnBuilder, TResult>> expression);
	}

	public interface ISelectColumnBuilder
    {
		object Is(string column);
		object Is(string column, string tableAlias);
		object Is<T>(Expression<Func<T, object>> expression);
	}
}

